using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors;
using System.Linq;

namespace Controllers
{
    public class EnemyController : Controller
    {
        public enum EnemyState
        {
            CHOOSEACTION,
            WAIT,
            ACTION,
            DEAD
        }

        public EnemyState CurrentEnemyState;

        public Enemy Enemy;

        public GameObject PlayerToAttack;

        public GameObject Selector;

        [SerializeField] protected GameObject HealthBar;
        protected HealthBar _healthBar;

        // Start is called before the first frame update
        protected override void Start()
        {
            _bm = GameObject.Find("BattleManager").GetComponent<BattleManager>();

            CreateHealthBar();
            _healthBar.SetSize((float)Enemy.HitPoints / Enemy.MaxHP);
            
            Selector.SetActive(false);

            CurrentEnemyState = EnemyState.CHOOSEACTION;

            _startPosition = transform.position;
        }

        protected virtual void CreateHealthBar()
        {
            GameObject healthBar = Instantiate(HealthBar) as GameObject;

            healthBar.transform.SetParent(transform);
            healthBar.transform.localPosition = new Vector2(0, .7f);

            _healthBar = healthBar.GetComponent<HealthBar>();
        }

        // TODO: - shorten this to be more in line with PlayerController
        protected override void Update()
        {
            _healthBar.SetSize((float)Enemy.HitPoints / Enemy.MaxHP);
            if (Enemy.IsDead)
                CurrentEnemyState = EnemyState.DEAD;

            switch (CurrentEnemyState)
            {
                case EnemyState.CHOOSEACTION:
                    TargetSelected(null);
                    break;

                case EnemyState.WAIT:
                    // wait for all actions to complete
                    if (!_bm.ListOfActions.Any())
                        CurrentEnemyState = EnemyState.CHOOSEACTION;
                    break;

                case EnemyState.ACTION:
                    StartCoroutine(TimeForAction());
                    break;

                case EnemyState.DEAD:
                    gameObject.SetActive(false);
                    _bm.PopTop();
                    _bm.EnemiesInBattle.Remove(gameObject);
                    _bm.CurrentBattleState = BattleManager.TurnState.TAKEACTIONS;
                    enabled = false;

                    break;
            }
        }

        public override void TargetSelected(GameObject obj)
        {
            HandleTurn action = new HandleTurn
            {
                Attacker = gameObject,
                AttackerSPD = Enemy.Speed,
                AttackerTag = "Enemy",
                Target = _bm.PlayersInBattle[Random.Range(0, _bm.PlayersInBattle.Count)]
            };

            _bm.CollectAction(action);
            CurrentEnemyState = EnemyState.WAIT;
        }

        protected override IEnumerator TimeForAction()
        {
            if (_actionStarted)
                yield break;

            _actionStarted = true;

            Vector2 playerPosition = new Vector2(PlayerToAttack.transform.position.x + 1.5f, PlayerToAttack.transform.position.y);

            while (MoveTowardsTarget(playerPosition))
                yield return null;

            yield return new WaitForSeconds(0.3f);
            // deal damage to target
            EngageTarget(PlayerToAttack);

            while (MoveTowardsTarget(_startPosition))
                yield return null;

            // remove action from list
            _bm.PopTop();
            _bm.CurrentBattleState = BattleManager.TurnState.TAKEACTIONS;

            _actionStarted = false;

            if (Enemy.IsDead)
                CurrentEnemyState = EnemyState.DEAD;
            else
                CurrentEnemyState = EnemyState.WAIT;
        }
        
        protected override void EngageTarget(GameObject target)
        {
            Player player = target.GetComponent<PlayerController>().Player;

            int damage = CalculateDamage(Enemy, player);
            player.TakeDamage(damage);
        }
    }
}

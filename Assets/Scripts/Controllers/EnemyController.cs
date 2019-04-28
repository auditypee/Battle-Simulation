using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors;
using System.Linq;
using UnityEngine.UI;
using Buttons;

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

        // Start is called before the first frame update
        protected override void Start()
        {
            Selector.SetActive(false);

            CurrentEnemyState = EnemyState.CHOOSEACTION;

            base.Start();
        }

        // TODO: - shorten this to be more in line with PlayerController
        protected override void Update()
        {
            _changeHealthBar.SetSize((float)Enemy.HitPoints / Enemy.MaxHP);
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
                    _bm.RemoveActorAction(gameObject);
                    _bm.EnemiesInBattle.Remove(gameObject);
                    //_createdButton.SetActive(false);
                    Destroy(gameObject);
                    _bm.CurrentBattleState = BattleManager.TurnState.TAKEACTIONS;
                    //enabled = false;

                    break;
            }
        }

        protected override void CreateHealthBar()
        {
            GameObject healthBar = Instantiate(HealthBar) as GameObject;
            _changeHealthBar = healthBar.GetComponent<HealthBar>();

            healthBar.transform.SetParent(transform);
            healthBar.transform.localPosition = new Vector2(0, .7f);
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

            // remove this action from list
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

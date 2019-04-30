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

        public bool TurnFinished = false;

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

                    break;

                case EnemyState.WAIT:

                    break;

                case EnemyState.ACTION:
                    if (PlayerToAttack == null)
                        CurrentEnemyState = EnemyState.CHOOSEACTION;
                    else
                        StartCoroutine(TimeForAction());

                    TurnFinished = false;
                    break;

                case EnemyState.DEAD:
                    _bm.RemoveActorAction(gameObject);
                    _bm.RemoveEnemy(gameObject);
                    Destroy(gameObject);
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

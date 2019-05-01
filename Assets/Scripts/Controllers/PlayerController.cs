using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors;
using System.Linq;
using UnityEngine.EventSystems;
using Buttons;

namespace Controllers
{
    public class PlayerController : Controller
    {
        public enum PlayerState
        {
            SELECTING,
            WAIT,
            ACTION,
            DEAD
        }

        public PlayerState CurrentPlayerState;

        public Player Player;

        public GameObject EnemyToAttack;
        
        protected override void Start()
        {
            CurrentPlayerState = PlayerState.SELECTING;

            base.Start();
        }
        
        protected override void Update()
        {
            _changeHealthBar.SetSize((float)Player.HitPoints / Player.MaxHP);
            if (Player.IsDead)
                CurrentPlayerState = PlayerState.DEAD;

            switch (CurrentPlayerState)
            {
                case PlayerState.SELECTING:
                    break;

                case PlayerState.WAIT:
                    // wait for all actions to complete
                    break;

                case PlayerState.ACTION:
                    if (EnemyToAttack == null)
                        CurrentPlayerState = PlayerState.SELECTING;
                    else
                        StartCoroutine(TimeForAction());
                    
                    break;

                case PlayerState.DEAD:
                    //_bm.RemoveActorAction(gameObject);
                    gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                    break;
            }
        }

        protected override void CreateHealthBar()
        {
            GameObject healthBar = Instantiate(HealthBar) as GameObject;
            _changeHealthBar = healthBar.GetComponent<HealthBar>();

            healthBar.transform.SetParent(GameObject.Find("ActionUIBG").transform, false);
        }
        
        protected override IEnumerator TimeForAction()
        {
            if (_actionStarted)
                yield break;

            _actionStarted = true;

            Vector2 playerPosition = new Vector2(EnemyToAttack.transform.position.x - 1.5f, EnemyToAttack.transform.position.y);

            while (MoveTowardsTarget(playerPosition))
                yield return null;

            yield return new WaitForSeconds(0.3f);
            // deal damage to target
            EngageTarget(EnemyToAttack);

            while (MoveTowardsTarget(_startPosition))
                yield return null;

            // remove action from list
            _bm.PopTop();

            _actionStarted = false;

            if (Player.IsDead)
                CurrentPlayerState = PlayerState.DEAD;
            else
                CurrentPlayerState = PlayerState.SELECTING;
        }

        protected override void EngageTarget(GameObject target)
        {
            Enemy enemy = target.GetComponent<EnemyController>().Enemy;

            int damage = CalculateDamage(Player, enemy);
            enemy.TakeDamage(damage);
        }
    }
}


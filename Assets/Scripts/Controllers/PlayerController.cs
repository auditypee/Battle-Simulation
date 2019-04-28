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

        public bool FinishedTurn = false;
        
        // Start is called before the first frame update
        protected override void Start()
        {
            Player = new Player("Venet");
            CurrentPlayerState = PlayerState.SELECTING;

            base.Start();
        }


        // Update is called once per frame
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
                    StartCoroutine(TimeForAction());
                    break;
                    //
                case PlayerState.DEAD:
                    gameObject.SetActive(false);
                    _bm.PopTop();

                    gameObject.GetComponent<SpriteRenderer>().color = Color.gray;

                    _bm.CurrentBattleState = BattleManager.TurnState.GAMEOVER;
                    break;
            }
        }
        
        protected override void CreateHealthBar()
        {
            
            //GameObject healthBar = Instantiate(HealthBar) as GameObject;
            _changeHealthBar = HealthBar.GetComponent<HealthBar>();

            //healthBar.transform.SetParent(GameObject.Find("ActionUIBG").transform);
        }
        
        // called from EnemySelectButtonHandler to create an action
        public override void TargetSelected(GameObject target)
        {
            HandleTurn action = new HandleTurn
            {
                Attacker = gameObject,
                AttackerTag = "Player",
                AttackerSPD = Player.Speed,
                Target = target
            };

            _bm.CollectAction(action);
            FinishedTurn = true;
            CurrentPlayerState = PlayerState.WAIT;
        }

        protected override IEnumerator TimeForAction()
        {
            FinishedTurn = false;
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
            _bm.CurrentBattleState = BattleManager.TurnState.TAKEACTIONS;

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


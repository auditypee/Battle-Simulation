using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Controllers;
using Buttons;

namespace States
{
    // implements the state interface, will be use to derive for every state of the battle
    // includes the owner of the state and the current state of the owner
    public abstract class BattleState : IState
    {
        protected BattleManager _owner;
        protected StateMachine _currentBattleState;

        protected BattleState(BattleManager owner)
        {
            _owner = owner;
            _currentBattleState = owner.CurrentBattleState;
        }

        public virtual void Enter() { AddListeners(); }
        public virtual void Execute() { }
        public virtual void Exit() { RemoveListeners(); }

        protected virtual void AddListeners() { }
        protected virtual void RemoveListeners() { }
    }

    
    //--- BattleStates ---//
    public class PlayerTurnState : BattleState
    {
        private GameObject _playerObj;
        private PlayerController _playerCon;

        public PlayerTurnState(BattleManager owner) : base(owner)
        {
            _playerObj = owner.Player;
            _playerCon = _playerObj.GetComponent<PlayerController>();
        }

        public override void Enter()
        {
            _owner.ActionsContainer.SetActive(true);

            base.Enter();
        }

        public override void Exit()
        {
            _owner.EnemySelectPanel.SetActive(false);
            _owner.ActionsContainer.SetActive(false); // remove once ally is in play <----------------

            base.Exit();
        }
        
        private void TakePlayerAction(GameObject target)
        {
            HandleTurn action = new HandleTurn
            {
                Attacker = _playerObj,
                AttackerTag = "Player",
                AttackerSPD = _playerCon.Player.Speed,
                Target = target
            };

            _owner.CollectAction(action);
            _playerCon.CurrentPlayerState = PlayerController.PlayerState.WAIT;

            _currentBattleState.ChangeState(new EnemyTurnState(_owner));
        }

        protected override void AddListeners()
        {
            EnemySelectButtonHandler.OnClickedTarget += TakePlayerAction;
        }
        protected override void RemoveListeners()
        {
            EnemySelectButtonHandler.OnClickedTarget -= TakePlayerAction;
        }
    }

    public class AllyTurnState : BattleState
    {
        public AllyTurnState(BattleManager owner) : base(owner)
        {
        }

        public override void Enter()
        {
            AddListener();
        }

        public override void Execute()
        {
            //throw new NotImplementedException();
        }

        public override void Exit()
        {
            _owner.ActionsContainer.SetActive(false);
            RemoveListener();
        }

        public void TakeAllyAction(GameObject target)
        {
            /*
            HandleTurn action = new HandleTurn
            {
                Attacker = _ally,
                AttackerTag = "Ally",
                AttackerSPD = _ally.GetComponent<AllyController>().Ally.Speed,
                Target = target
            };

            CollectAction(action);
            CurrentBattleState = TurnState.ENEMYTURN;
            */
        }

        private void AddListener()
        {
            EnemySelectButtonHandler.OnClickedTarget += TakeAllyAction;
        }

        private void RemoveListener()
        {
            EnemySelectButtonHandler.OnClickedTarget -= TakeAllyAction;
        }
    }

    public class EnemyTurnState : BattleState
    {
        private List<GameObject> _enemies;

        public EnemyTurnState(BattleManager owner) : base(owner)
        {
            _enemies = owner.Enemies;
        }

        public override void Execute()
        {
            foreach (var enemy in _enemies)
                TakeEnemyAction(enemy);

            _currentBattleState.ChangeState(new ResolveTurnsState(_owner));
        }

        private void TakeEnemyAction(GameObject enemy)
        {
            HandleTurn action = new HandleTurn
            {
                Attacker = enemy,
                AttackerTag = "Enemy",
                AttackerSPD = enemy.GetComponent<EnemyController>().Enemy.Speed,
                Target = _owner.GetPlayer()
            };

            _owner.CollectAction(action);
            enemy.GetComponent<EnemyController>().CurrentEnemyState = EnemyController.EnemyState.WAIT;
        }
    }

    public class ResolveTurnsState : BattleState
    {
        public ResolveTurnsState(BattleManager owner) : base(owner)
        {
        }

        public override void Enter()
        {
            _owner.ActionsContainer.SetActive(false);
            
        }

        public override void Execute()
        {
            if (!_owner.ListOfActions.Any())
                _currentBattleState.ChangeState(new PlayerTurnState(_owner));

            else
            {
            TakeActions();
                _currentBattleState.ChangeState(new PerformActionState(_owner));
            }
        }

        #region Converting HandleTurn to an Action
        private void TakeActions()
        {
            HandleTurn currentActor = _owner.ListOfActions.First();
            if (currentActor.AttackerTag == "Enemy")
                EnemyDoesAction(currentActor);

            if (currentActor.AttackerTag == "Player")
                PlayerDoesAction(currentActor);

            if (currentActor.AttackerTag == "Ally")
                AllyDoesAction(currentActor);
        }

        private static void EnemyDoesAction(HandleTurn currentActor)
        {
            EnemyController currentEnemy = currentActor.Attacker.GetComponent<EnemyController>();
            currentEnemy.CurrentEnemyState = EnemyController.EnemyState.ACTION;
            currentEnemy.PlayerToAttack = currentActor.Target;
        }

        private static void PlayerDoesAction(HandleTurn currentActor)
        {
            PlayerController currentPlayer = currentActor.Attacker.GetComponent<PlayerController>();
            currentPlayer.CurrentPlayerState = PlayerController.PlayerState.ACTION;
            currentPlayer.EnemyToAttack = currentActor.Target;
        }

        private static void AllyDoesAction(HandleTurn currentActor)
        {
            AllyController currentAlly = currentActor.Attacker.GetComponent<AllyController>();
            //currentAlly.CurrentAllyState = AllyController.AllyState.ACTION;
            //currentAlly.EnemyToAttack = currentActor.Target;
        }
        #endregion
    }

    public class PerformActionState : BattleState
    {
        public PerformActionState(BattleManager owner) : base(owner)
        {
        }
        
        public override void Execute()
        {
            _currentBattleState.ChangeState(new ResolveTurnsState(_owner));

            if (_owner.AllEnemiesDead())
                _currentBattleState.ChangeState(new EndBattleState(_owner));

            if (_owner.AllPlayersDead())
                _currentBattleState.ChangeState(new GameOverState(_owner));
        }

        public override void Exit()
        {
            //_owner.PopTop();
        }
    }

    public class EndBattleState : BattleState
    {
        public EndBattleState(BattleManager owner) : base(owner)
        {
        }

        public override void Enter()
        {
            Debug.Log("A winner is you!");
        }

        public override void Execute()
        {
            base.Execute();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    public class GameOverState : BattleState
    {
        public GameOverState(BattleManager owner) : base(owner)
        {
        }

        public override void Enter()
        {
            Debug.Log("You Died");
        }

        public override void Execute()
        {
            base.Execute();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}

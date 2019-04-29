using Buttons;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Actors;

namespace Controllers
{
    public class BattleManager : MonoBehaviour
    {
        public enum TurnState
        {
            PLAYERTURN,
            ALLYTURN,
            ENEMYTURN,
            RESOLVETURNORDER,
            TAKEACTIONS,
            PERFORMACTION,
            GAMEOVER,
            ENDBATTLE
        }

        public TurnState CurrentBattleState;

        private GameObject _player;
        private GameObject _ally;
        private List<GameObject> _enemies = new List<GameObject>();

        [SerializeField] private List<HandleTurn> _listOfActions = new List<HandleTurn>();
        public List<HandleTurn> ListOfActions
        {
            get { return _listOfActions; }
            private set { }
        }

        public GameObject ActionsContainer;
        public GameObject EnemySelectPanel;
        public Transform TargetSpacer;
        public GameObject TargetButton;

        private BattleGround battleGround;

        private void Awake()
        {
            battleGround = GetComponent<BattleGround>();
            battleGround.SetupScene();

            _player = battleGround.Player;
            _ally = battleGround.Ally;
            _enemies = battleGround.Enemies;
        }

        void Start()
        {
            CurrentBattleState = TurnState.PLAYERTURN;

            EnemySelectPanel.SetActive(false);
        }
        
        void Update()
        {
            switch (CurrentBattleState)
            {
                case TurnState.PLAYERTURN:
                    ActionsContainer.SetActive(true);
                    _player.GetComponent<PlayerController>().CurrentPlayerState = PlayerController.PlayerState.SELECTING;
                    
                    if (_player.GetComponent<PlayerController>().TurnFinished)
                        CurrentBattleState = TurnState.ALLYTURN;
                    break;

                case TurnState.ALLYTURN:

                    CurrentBattleState = TurnState.ENEMYTURN;
                    break;
                case TurnState.ENEMYTURN:
                    foreach (var enemy in _enemies)
                        enemy.GetComponent<EnemyController>().CurrentEnemyState = EnemyController.EnemyState.CHOOSEACTION;
                    //new WaitForSeconds(3f);
                    CurrentBattleState = TurnState.RESOLVETURNORDER;
                    break;

                case TurnState.RESOLVETURNORDER:
                    ActionsContainer.SetActive(false);

                    if (!_listOfActions.Any())
                        CurrentBattleState = TurnState.PLAYERTURN;
                    else
                    {
                        TakeActions();
                        CurrentBattleState = TurnState.PERFORMACTION;
                    }
                    break;

                case TurnState.PERFORMACTION:
                    CurrentBattleState = TurnState.RESOLVETURNORDER;
                    if (AllPlayersDead())
                        CurrentBattleState = TurnState.GAMEOVER;
                    if (AllEnemiesDead())
                        CurrentBattleState = TurnState.ENDBATTLE;
                    break;

                case TurnState.ENDBATTLE:
                    Debug.Log("A winner is you!");
                    break;

                case TurnState.GAMEOVER:
                    Debug.Log("Game Over");
                    break;
            }
        }

        private void TakeActions()
        {
            HandleTurn currentActor = _listOfActions.First();
            if (currentActor.AttackerTag == "Enemy")
                EnemyDoesAction(currentActor);

            if (currentActor.AttackerTag == "Player")
                PlayerDoesAction(currentActor);
            
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
        
        public void CreateEnemyButtons()
        {
            foreach (Transform child in TargetSpacer)
                Destroy(child.gameObject);
            foreach (GameObject enemy in _enemies)
            {
                // create buttons that contain the data for every enemy in the battle
                GameObject newButton = Instantiate(TargetButton) as GameObject;
                EnemySelectButtonHandler enemyButtonHandler = newButton.GetComponent<EnemySelectButtonHandler>();

                Text buttonText = newButton.GetComponentInChildren<Text>();
                buttonText.text = enemy.GetComponent<EnemyController>().Enemy.Name;

                enemyButtonHandler.EnemyPrefab = enemy;
                newButton.transform.SetParent(TargetSpacer);
            }
        }
        


        // collects actions from actors and sorts by speed
        public void CollectAction(HandleTurn action)
        {
            _listOfActions.Add(action);
            _listOfActions = _listOfActions.OrderByDescending(s => s.AttackerSPD).ToList();
        }

        // removes the actor if they are on the ListOfActions
        public void RemoveActorAction(GameObject actor)
        {
            _listOfActions.RemoveAll(a => a.Attacker == actor);
        }

        // takes out the top of the list
        public void PopTop()
        {
            _listOfActions.RemoveAt(0);
        }

        public bool AllPlayersDead()
        {
            //_ally.GetComponent<AllyController>().Ally.IsDead;
            return _player.GetComponent<PlayerController>().Player.IsDead;
        }

        public bool AllEnemiesDead()
        {
            return !_enemies.Any();
        }

        public int NumOfActors()
        {
            int num = _enemies.Count;
            if (_player != null)
                num++;
            if (_ally != null)
                num++;

            return num;
        }

        public void RemoveEnemy(GameObject enemy)
        {
            _enemies.Remove(enemy);
        }

        public GameObject GetPlayer()
        {
            return _player;
        }

        public GameObject GetRandPlayer()
        {
            if (Random.Range(0, 2) == 0)
                return !_player.GetComponent<PlayerController>().Player.IsDead ? _player : _ally; // if player isn't dead
            else
                return !_ally.GetComponent<AllyController>().Ally.IsDead ? _ally : _player; // if ally isn't dead
        }
    }
}

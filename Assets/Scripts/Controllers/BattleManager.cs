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
            WAITFORACTIONS,
            TAKEACTIONS,
            PERFORMACTION,
            GAMEOVER
        }

        public TurnState CurrentBattleState;

        public static BattleManager instance = null; // allows static call of the BattleManager

        public List<GameObject> EnemiesInBattle = new List<GameObject>();
        public List<GameObject> PlayersInBattle = new List<GameObject>();

        [SerializeField] private List<HandleTurn> _listOfActions = new List<HandleTurn>();
        public List<HandleTurn> ListOfActions
        {
            get { return _listOfActions; }
            private set { }
        }

        public GameObject EnemyToCreate;

        public GameObject ActionsContainer;
        public GameObject EnemySelectPanel;
        public Transform TargetSpacer;
        public GameObject TargetButton;
        
        private static Vector2 _position1;
        private static Vector2 _position2;
        private static Vector2 _position3;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        private void InitPositions()
        {
            Vector2 bottomLeftOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
            Vector2 topRightOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            _position1 = new Vector2(topRightOfScreen.x - 1f, topRightOfScreen.y - 1.5f);
            _position1 -= Vector2.right * transform.localScale.x;
            _position2 = new Vector2(topRightOfScreen.x - 1f, 0);
            _position2 -= Vector2.right * transform.localScale.x;
        }

        void Start()
        {
            InitPositions();

            
            PlayersInBattle.AddRange(GameObject.FindGameObjectsWithTag("Player"));

            CurrentBattleState = TurnState.WAITFORACTIONS;

            EnemySelectPanel.SetActive(false);

            CreateEnemy(new Enemy("Slime", 2, 5, 5, 5, 6, 3, 3), _position1);
            CreateEnemy(new Enemy("Orc", 5, 15, 20, 20, 1, 10, 10), _position2);
        }
        
        void Update()
        {
            switch (CurrentBattleState)
            {
                case TurnState.WAITFORACTIONS:
                    ActionsContainer.SetActive(true);
                    
                    int numOfActors = EnemiesInBattle.Count + PlayersInBattle.Count;

                    // everyone created an action
                    if (_listOfActions.Count >= numOfActors)
                    {
                        CurrentBattleState = TurnState.TAKEACTIONS;
                        ActionsContainer.SetActive(false);
                    }

                    break;

                // TODO: - could code this better
                case TurnState.TAKEACTIONS: // should stay in this state until all actions have been used
                                            // if all actions are depleted, go wait for more actions
                    if (!_listOfActions.Any())
                        CurrentBattleState = TurnState.WAITFORACTIONS;
                    // keep using up actions
                    else
                    {
                        TakeActions();
                        CurrentBattleState = TurnState.PERFORMACTION;
                    }
                    break;

                case TurnState.PERFORMACTION:
                    break;

                case TurnState.GAMEOVER:
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

        public void EnemySelected(GameObject target)
        {
            Debug.Log(target.GetComponent<EnemyController>().Enemy.Name);
        }

        private void CreateEnemy(Enemy enemy, Vector2 position)
        {
            GameObject newEnemy = Instantiate(EnemyToCreate) as GameObject;

            // position of enemy
            newEnemy.transform.position = position;
            // values of the enemy
            newEnemy.GetComponent<EnemyController>().Enemy = enemy;
            EnemiesInBattle.Add(newEnemy);
        }
        
        
        public void CreateEnemyButtons()
        {
            foreach (Transform child in TargetSpacer)
                Destroy(child.gameObject);
            foreach (GameObject enemy in EnemiesInBattle)
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
    }
}

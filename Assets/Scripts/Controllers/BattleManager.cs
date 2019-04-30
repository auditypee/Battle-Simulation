using Buttons;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Actors;
using States;

namespace Controllers
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private List<HandleTurn> _listOfActions = new List<HandleTurn>();

        private BattleGround battleGround;

        private GameObject _player;
        private GameObject _ally;
        private List<GameObject> _enemies;

        public GameObject Player
        {
            get { return _player; }
            private set { }
        }
        public GameObject Ally
        {
            get { return _ally; }
            private set { }
        }
        public List<GameObject> Enemies
        {
            get { return _enemies; }
            private set { }
        }
        public List<HandleTurn> ListOfActions
        {
            get { return _listOfActions; }
            private set { }
        }
        public StateMachine CurrentBattleState = new StateMachine();

        public GameObject ActionsContainer;
        public GameObject EnemySelectPanel;
        public Transform TargetSpacer;
        public GameObject TargetButton;
        
        private void Awake()
        {
            battleGround = GetComponent<BattleGround>();
            battleGround.SetupScene();

            _player = battleGround.Player;
            _ally = battleGround.Ally;
            _enemies = battleGround.Enemies;
        }

        private void Start()
        {
            EnemySelectPanel.SetActive(false);
            CurrentBattleState.ChangeState(new PlayerTurnState(this));
        }
        
        private void Update()
        {
            CurrentBattleState.Update();
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
            return !Enemies.Any();
        }

        public int NumOfActors()
        {
            int num = Enemies.Count;
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

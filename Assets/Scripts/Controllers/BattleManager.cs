using Buttons;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Actors;
using States;
using Menu;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private List<HandleTurn> _listOfActions = new List<HandleTurn>();

        private BattleGround battleGround;

        private GameObject _player;
        private GameObject _ally;
        private List<GameObject> _enemies;

        private GameManager _gameManager = GameManager.Instance;

        public static BattleManager Instance = null;

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
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

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

        public bool AllyIsDead()
        {
            return _player.GetComponent<PlayerController>().Player.IsDead;
        }

        public bool PlayerIsDead()
        {
            return _ally.GetComponent<AllyController>().Ally.IsDead;
        }

        public bool AllPlayersDead()
        {
            //_ally.GetComponent<AllyController>().Ally.IsDead;
            return _player.GetComponent<PlayerController>().Player.IsDead;
        }

        public bool AllEnemiesDead()
        {
            foreach (var enemy in _enemies)
            {
                if (!enemy.GetComponent<EnemyController>().Enemy.IsDead)
                    return false;
            }
            return true;
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

        public void PlayerWon()
        {
            _gameManager.Player = _player.GetComponent<PlayerController>().Player;
            SceneManager.LoadScene("SetupBattle");
        }

        public void PlayerLost()
        {

        }
    }
}

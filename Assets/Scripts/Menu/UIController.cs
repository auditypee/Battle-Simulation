using System.Collections.Generic;
using UnityEngine;
using Actors;
using Buttons;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class UIController : MonoBehaviour
    {
        private Player _player;
        private List<Enemy> _enemies = new List<Enemy>();

        public Player Player
        {
            get { return _player; }
            private set { }
        }

        public List<Enemy> Enemies
        {
            get { return _enemies; }
            private set { }
        }

        private readonly int MAX_ENEMIES = 4;

        public GameObject PlayerInfoPanel;
        public GameObject BattleCreationUI;

        private GameObject _playerInfoPanel;
        private GameObject _playerPanel;

        private GameObject _enemiesPanel;

        private PlayerInfo _playerInfo;
        private EnemyInfo _enemyInfo;

        private void Awake()
        {
            GameObject ui = Instantiate(BattleCreationUI) as GameObject;

            _playerPanel = GameObject.Find("PlayerPanel");
            _enemiesPanel = GameObject.Find("EnemiesPanel");

            _enemyInfo = _enemiesPanel.GetComponent<EnemyInfo>();

            if (GameManager.Instance.Player != null)
            {
                CreatePlayer(GameManager.Instance.Player);
            }
        }

        // creates the player and shows their stats
        private void CreatePlayer(Player player)
        {
            _player = player;

            if (_playerInfoPanel == null)
            {
                _playerInfoPanel = Instantiate(PlayerInfoPanel) as GameObject;
                _playerInfoPanel.transform.SetParent(_playerPanel.transform, false);
                _playerInfo = _playerInfoPanel.GetComponent<PlayerInfo>();

                _playerInfo.SetupPanels(_player);
            }
            else
            {
                _playerInfo.UpdateStats(_player);
            }
        }

        private void PlayerLevelUp()
        {
            _player.NextLevel();
            _playerInfo.UpdateStats(_player);
        }

        private void PlayerSetLevel(int n)
        {
            _player.SetLvl(n);
            _playerInfo.UpdateStats(_player);
        }

        private void CreateEnemy()
        {
            if (_enemies.Count < MAX_ENEMIES)
            {
                Enemy enemy = _enemyInfo.CollectEnemyInfo();
                _enemies.Add(enemy);
            }
        }

        private void DeleteEnemy(Enemy enemy)
        {
            _enemies.Remove(enemy);
        }

        private void LoadBattleScene()
        {
            if (_player != null && _enemies.Any())
            {
                GameManager.Instance.Player = Player;
                GameManager.Instance.Enemies = Enemies;

                Debug.Log(GameManager.Instance.Player.Name + " Created");

                SceneManager.LoadScene("Battle");
            }
            else
            {
                Debug.Log("Missing");
            }
        }

        private void OnEnable()
        {
            CreatePlayerButtonHandler.OnClickCreate += CreatePlayer;
            LvlUpPlayerButtonHandler.OnClickLevelUp += PlayerLevelUp;
            LvlUpPlayerButtonHandler.OnClickLevelSet += PlayerSetLevel;
            ResetCreateEnemyButtonHandler.OnClickCreate += CreateEnemy;
            EnemyPortraitHandler.OnClickDelete += DeleteEnemy;
            StartEncounterButtonHandler.OnClickLoadScene += LoadBattleScene;
        }

        private void OnDisable()
        {
            CreatePlayerButtonHandler.OnClickCreate -= CreatePlayer;
            LvlUpPlayerButtonHandler.OnClickLevelUp -= PlayerLevelUp;
            LvlUpPlayerButtonHandler.OnClickLevelSet -= PlayerSetLevel;
            ResetCreateEnemyButtonHandler.OnClickCreate -= CreateEnemy;
            EnemyPortraitHandler.OnClickDelete -= DeleteEnemy;
            StartEncounterButtonHandler.OnClickLoadScene -= LoadBattleScene;
        }
    }
}

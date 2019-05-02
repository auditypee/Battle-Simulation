﻿using System.Collections.Generic;
using UnityEngine;
using Actors;
using Buttons;

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
        public GameObject PlayerPanel;

        public GameObject EnemiesPanel;

        private GameObject _playerInfoPanel;

        private PlayerInfo _playerInfo;
        private EnemyInfo _enemyInfo;

        private void Awake()
        {
            _enemyInfo = EnemiesPanel.GetComponent<EnemyInfo>();
        }

        // creates the player and shows their stats
        private void CreatePlayer(Player player)
        {
            _player = player;

            if (_playerInfoPanel == null)
            {
                _playerInfoPanel = Instantiate(PlayerInfoPanel) as GameObject;
                _playerInfoPanel.transform.SetParent(PlayerPanel.transform, false);
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
            _player.CheckLevelUp(_player.ExpNeededToLvlUp());
            _playerInfo.UpdateStats(_player);
        }

        private void PlayerLevelDown()
        {
            _player.CheckLevelDown();
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

        private void OnEnable()
        {
            CreatePlayerButtonHandler.OnClickCreate += CreatePlayer;
            LvlUpPlayerButtonHandler.OnClickLevelUp += PlayerLevelUp;
            LvlUpPlayerButtonHandler.OnClickLevelDown += PlayerLevelDown;
            CreateEnemyButtonHandler.OnClickCreate += CreateEnemy;
            EnemyPortraitHandler.OnClickDelete += DeleteEnemy;
        }

        private void OnDisable()
        {
            CreatePlayerButtonHandler.OnClickCreate -= CreatePlayer;
            LvlUpPlayerButtonHandler.OnClickLevelUp -= PlayerLevelUp;
            LvlUpPlayerButtonHandler.OnClickLevelDown -= PlayerLevelDown;
            CreateEnemyButtonHandler.OnClickCreate -= CreateEnemy;
            EnemyPortraitHandler.OnClickDelete -= DeleteEnemy;
        }
    }
}

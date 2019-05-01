using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Actors;
using Buttons;

namespace Menu
{
    public class UIController : MonoBehaviour
    {
        private Player _player;
        private List<Enemy> _enemies = new List<Enemy>();

        public GameObject PlayerInfoPanel;
        public GameObject PlayerPanel;

        private GameObject _playerInfoPanel;

        private PlayerInfo _playerInfo;

        private void CreatePlayer(Player player)
        {
            _player = player;

            if (_playerInfoPanel == null)
            {
                _playerInfoPanel = Instantiate(PlayerInfoPanel) as GameObject;
                _playerInfoPanel.transform.SetParent(PlayerPanel.transform, false);
                _playerInfo = _playerInfoPanel.GetComponent<PlayerInfo>();

                _playerInfo.SetupPanels(_player);
                Debug.Log(player.Name);
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

        private void OnEnable()
        {
            CreatePlayerButtonHandler.OnClickCreate += CreatePlayer;
            LvlUpPlayerButtonHandler.OnClickLevelUp += PlayerLevelUp;
            LvlUpPlayerButtonHandler.OnClickLevelDown += PlayerLevelDown;
        }

        private void OnDisable()
        {
            CreatePlayerButtonHandler.OnClickCreate -= CreatePlayer;
            LvlUpPlayerButtonHandler.OnClickLevelUp -= PlayerLevelUp;
            LvlUpPlayerButtonHandler.OnClickLevelDown -= PlayerLevelDown;
        }
    }
}

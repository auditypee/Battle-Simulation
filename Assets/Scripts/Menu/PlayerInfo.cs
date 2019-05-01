using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Actors;

namespace Menu
{
    public class PlayerInfo : MonoBehaviour
    {
        private Player _player;

        public GameObject PlayerStatsPanel;
        public GameObject PlayerWeaponPanel;
        public GameObject PlayerArmorPanel;
        public GameObject PlayerLevelPanel;
        public GameObject PlayerImage;

        public void SetupPanels(Player player)
        {
            _player = player;
            SetupPlayerStats();
        }

        private void SetupPlayerStats()
        {
            Text statsText = PlayerStatsPanel.GetComponentInChildren<Text>();

            string playerStats = "Name: " + _player.Name +
                "\nLevel: " + _player.Level +
                "\nExperience: " + _player.Experience +
                "\nHP: " + _player.MaxHP +
                "\nMP: " + _player.MaxMana +
                "\nAttack: " + _player.Attack +
                "\nDefense: " + _player.Defense +
                "\nSpeed: " + _player.Speed;

            statsText.text = playerStats;
        }

        // TODO: - implement equipping weapons and armor
        // update stats will be used to show equipping of these items in the future
        public void UpdateStats(Player player)
        {
            _player = player;

            Text statsText = PlayerStatsPanel.GetComponentInChildren<Text>();
            statsText.text = string.Empty;

            string playerStats = "Name: " + _player.Name +
                "\nLevel: " + _player.Level +
                "\nExperience: " + _player.Experience +
                "\nHP: " + _player.MaxHP +
                "\nMP: " + _player.MaxMana +
                "\nAttack: " + _player.Attack +
                "\nDefense: " + _player.Defense +
                "\nSpeed: " + _player.Speed;

            statsText.text = playerStats;
        }
    }
}

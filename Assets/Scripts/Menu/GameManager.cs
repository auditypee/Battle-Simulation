using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Actors;
using Buttons;

namespace Menu
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;

        public Player Player { get; set; }
        public Ally Ally { get; set; }
        public List<Enemy> Enemies { get; set; } = new List<Enemy>();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (Input.GetKey("escape"))
            {
                Debug.Log("Escape key pressed");
                Application.Quit();
            }
        }

        public void SetupBattleSetup(Player player)
        {
            Player = player;
            //_ally = ally;
            
            Debug.Log("Back to Thing");
        }
    }
}


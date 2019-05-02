using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Actors;
using Buttons;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;

        private UIController _uiScript;

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

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            _uiScript = GetComponent<UIController>();

            
        }

        private void LoadBattleScene()
        {
            _player = _uiScript.Player;
            _enemies = _uiScript.Enemies;

            if (_player != null && _enemies.Any())
            {
                SceneManager.LoadScene("Battle");
            }
            else
            {
                Debug.Log("Missing");
            }
        }

        private void OnEnable()
        {
            StartEncounterButtonHandler.OnClickLoadScene += LoadBattleScene;
        }

        private void OnDisable()
        {
            StartEncounterButtonHandler.OnClickLoadScene -= LoadBattleScene;
        }
    }
}


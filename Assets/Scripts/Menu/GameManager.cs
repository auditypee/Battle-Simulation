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

        public GameObject BattleCreationUI;

        private UIController _uiScript;

        private Player _player;
        private Ally _ally;
        private List<Enemy> _enemies = new List<Enemy>();

        public Player Player
        {
            get { return _player; }
            private set { }
        }
        public Ally Ally
        {
            get { return _ally; }
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

            GameObject ui = Instantiate(BattleCreationUI) as GameObject;
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
            _player = player;
            //_ally = ally;

            GameObject ui = Instantiate(BattleCreationUI) as GameObject;
            Debug.Log(ui.name);

            Debug.Log("Back to Thing");
        }

        private void LoadBattleScene()
        {
            _player = _uiScript.Player;
            _enemies = _uiScript.Enemies;

            if (Player != null && _enemies.Any())
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


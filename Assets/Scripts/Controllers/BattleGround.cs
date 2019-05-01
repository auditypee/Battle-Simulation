using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Actors;

namespace Controllers
{
    public class BattleGround : MonoBehaviour
    {
        private static Vector2 _position1r;
        private static Vector2 _position2r;
        private static Vector2 _position3r;
        private static Vector2 _position1l;
        private static Vector2 _position2l;

        public GameObject InstPlayer;
        public GameObject InstAlly;
        public GameObject InstEnemy;

        [HideInInspector] public GameObject Player;
        [HideInInspector] public GameObject Ally;
        [HideInInspector] public List<GameObject> Enemies;

        private void InitPositions()
        {
            Vector2 bottomLeftOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
            Vector2 topRightOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            _position1r = new Vector2(topRightOfScreen.x - 1f, topRightOfScreen.y - 1.5f);
            _position1r -= Vector2.right * transform.localScale.x;
            _position2r = new Vector2(topRightOfScreen.x - 1f, 1.5f);
            _position2r -= Vector2.right * transform.localScale.x;
            _position3r = new Vector2(topRightOfScreen.x - 1f, -1.5f);
            _position3r -= Vector2.right * transform.localScale.x;

            _position1l = new Vector2(bottomLeftOfScreen.x + 1f, topRightOfScreen.y - 1.5f);
            _position1l -= Vector2.left * transform.localScale.x;
            _position2l = new Vector2(bottomLeftOfScreen.x + 1f, 0);
            _position2l -= Vector2.left * transform.localScale.x;
        }

        private void CreatePlayer(Player player, Vector2 position)
        {
            GameObject newPlayer = Instantiate(InstPlayer) as GameObject;

            newPlayer.transform.position = position;
            newPlayer.GetComponent<PlayerController>().Player = player;

            Player = newPlayer;
        }

        private void CreateAlly(Ally ally, Vector2 position)
        {
            GameObject newAlly = Instantiate(InstAlly) as GameObject;

            newAlly.transform.position = position;
            newAlly.GetComponent<AllyController>().Ally = ally;

            Player = newAlly;
        }

        private void CreateEnemy(Enemy enemy, Vector2 position)
        {
            GameObject newEnemy = Instantiate(InstEnemy) as GameObject;

            // position of enemy
            newEnemy.transform.position = position;
            // values of the enemy
            newEnemy.GetComponent<EnemyController>().Enemy = enemy;
            Enemies.Add(newEnemy);
        }

        public void SetupScene()
        {
            InitPositions();

            CreatePlayer(new Player("Venet"), _position1l);
            //CreateAlly(new Ally("Ally"), _position2l);
            CreateEnemy(new Enemy("Slime", 2, 5, 5, 5, 6, 3, 3), _position1r);
            CreateEnemy(new Enemy("Orc", 5, 15, 8, 10, 1, 10, 10), _position2r);
            CreateEnemy(new Enemy("Weakling", 1, 1, 1, 1, 1, 1, 1), _position3r);
        }
    }
}

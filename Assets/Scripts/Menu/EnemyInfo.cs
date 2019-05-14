using UnityEngine;
using UnityEngine.UI;
using Actors;
using Buttons;

namespace Menu
{
    public class EnemyInfo : MonoBehaviour
    {
        public InputField Name;
        public InputField Level;
        public InputField ExpReward;
        public InputField HitPoints;
        public InputField Attack;
        public InputField Defense;
        public InputField Speed;

        public Transform EnemyPresetSpacer;
        public GameObject EnemyPreset;

        public Transform EnemyPortraitSpacer;
        public GameObject EnemyPortrait;

        private readonly Enemy _orc = new Enemy("Orc", 10, 20, 50, 15, 10, 10);
        private readonly Enemy _weakling = new Enemy("Weakling", 1, 1, 5, 1, 1, 1);
        private readonly Enemy _bahamut = new Enemy("Bahamut", 99, 999999, 9999, 99, 99, 99);
        private readonly Enemy _soldier = new Enemy("Soldier", 50, 125000, 1000, 50, 60, 50);

        private void Start()
        {
            CreateEnemyPreset(_orc);
            CreateEnemyPreset(_weakling);
            CreateEnemyPreset(_bahamut);
            CreateEnemyPreset(_soldier);
        }

        private void CreateEnemyPreset(Enemy enemy)
        {
            GameObject enemyPreset = Instantiate(EnemyPreset) as GameObject;
            enemyPreset.transform.SetParent(EnemyPresetSpacer, false);
            enemyPreset.GetComponentInChildren<Text>().text = enemy.Name;

            EnemyPresetButtonHandler handler = enemyPreset.GetComponent<EnemyPresetButtonHandler>();
            handler.Enemy = enemy;
        }

        public Enemy CollectEnemyInfo()
        {
            // default value is Name: Slime, Level: 1, ExpReward: 5, HP: 5, Attack: 5, Defense: 3, Speed: 3
            string name = Name.text;
            if (name == string.Empty)
                name = "Slime";
            int level = int.TryParse(Level.text, out level) ? level : 1;
            int xpReward = int.TryParse(ExpReward.text, out xpReward) ? xpReward : 5;
            int hp = int.TryParse(HitPoints.text, out hp) ? hp : 5;
            int atk = int.TryParse(Attack.text, out atk) ? atk : 5;
            int def = int.TryParse(Defense.text, out def) ? def : 3;
            int spd = int.TryParse(Speed.text, out spd) ? spd : 3;

            //Debug.Log("Name: " + name + "| Level: " + level);
            Enemy enemy = new Enemy(name, level, xpReward, hp, atk, def, spd);
            ResetInputs();
            CreateEnemyPortrait(enemy);

            return enemy;
        }

        private void CreateEnemyPortrait(Enemy enemy)
        {
            GameObject enemyPortrait = Instantiate(EnemyPortrait) as GameObject;
            enemyPortrait.transform.GetChild(1).GetComponent<Text>().text = enemy.Name;

            enemyPortrait.GetComponent<EnemyPortraitHandler>().Enemy = enemy;
            enemyPortrait.transform.SetParent(EnemyPortraitSpacer, false);
        }

        private void ResetInputs()
        {
            Name.text = "";
            Level.text = "";
            ExpReward.text = "";
            HitPoints.text = "";
            Attack.text = "";
            Defense.text = "";
            Speed.text = "";
        }

        private void PresetFields(Enemy enemy)
        {
            Name.text = enemy.Name;
            Level.text = enemy.Level.ToString();
            ExpReward.text = enemy.Experience.ToString();
            HitPoints.text = enemy.HitPoints.ToString();
            Attack.text = enemy.Attack.ToString();
            Defense.text = enemy.Defense.ToString();
            Speed.text = enemy.Speed.ToString();
        }

        private void OnEnable()
        {
            EnemyPresetButtonHandler.OnClickPopulate += PresetFields;
            ResetCreateEnemyButtonHandler.OnClickReset += ResetInputs;
        }

        private void OnDisable()
        {
            EnemyPresetButtonHandler.OnClickPopulate -= PresetFields;
            ResetCreateEnemyButtonHandler.OnClickReset -= ResetInputs;
        }
    }
}

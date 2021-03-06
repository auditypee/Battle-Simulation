﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Actors
{
    public abstract class Actor
    {
        #region Properties
        [SerializeField] private string _name;
        [SerializeField] private int _hitPoints;
        [SerializeField] private int _maxHP;
        [SerializeField] private int _mana;
        [SerializeField] private int _maxMana;
        [SerializeField] private int _attack;
        [SerializeField] private int _defense;
        [SerializeField] private int _speed;
        
        [SerializeField] private int _level;
        [SerializeField] private int _experience;
        private int _nextLvlUp;
        private readonly float _nextLvlModifier = 1.15f;

        private readonly int MAX_LEVEL = 99;

        public bool IsDead => HitPoints <= 0;

        public string Name { get => _name; protected set => _name = value; }
        public int Level { get => _level; protected set => _level = value; }
        public int Experience { get => _experience; protected set => _experience = value; }
        public int HitPoints { get => _hitPoints; protected set => _hitPoints = value; }
        public int MaxHP { get => _maxHP; protected set => _maxHP = value; }
        public int Mana { get => _mana; protected set => _mana = value; }
        public int MaxMana { get => _maxMana; protected set => _maxMana = value; }
        public int Attack { get => _attack; protected set => _attack = value; }
        public int Defense { get => _defense; protected set => _defense = value; }
        public int Speed { get => _speed; protected set => _speed = value; }
        public int NextLvlUp { get => _nextLvlUp; protected set => _nextLvlUp = value; }
        #endregion

        // empty constructor for created enemies
        protected Actor() { }

        // default hero, base values for levelling
        protected Actor(string name)
        {
            if (name == null)
                Name = "Hero";
            else
                Name = name;

            // default start values
            Level = 1;
            Experience = 0;
            HitPoints = 10;
            MaxHP = 10;
            Mana = 5;
            MaxMana = 5;
            Attack = 10;
            Defense = 5;
            Speed = 5;

            _nextLvlUp = 10;
        }

        #region Setters
        public void TakeDamage(int damage)
        {
            HitPoints -= damage;

            if (IsDead)
            {
                HitPoints = 0;
            }
        }

        public void CompletelyHeal()
        {
            HitPoints = MaxHP;
        }

        public void Heal(int hpToHeal)
        {
            HitPoints += hpToHeal;

            if (HitPoints > MaxHP)
                CompletelyHeal();
        }

        public void CompletelyRegainMana()
        {
            Mana = MaxMana;
        }

        public void RegainMana(int manaToRegain)
        {
            Mana += manaToRegain;

            if (Mana > MaxMana)
                CompletelyRegainMana();
        }

        // gain experience to level up character, hidden
        protected void GainExperience(int expGained)
        {
            Experience += expGained;
            if (Level != MAX_LEVEL)
            {
                while (Experience >= _nextLvlUp)
                {
                    LevelUp();

                    Debug.Log("Congratulations! You are now level " + Level);

                    //float t = Mathf.Pow(_nextLvlModifier, Level);
                    _nextLvlUp = (int)Mathf.Floor(_nextLvlUp * _nextLvlModifier + 8);

                    Debug.Log("Next level up at " + _nextLvlUp);
                }
            }
        }

        private void LevelUp()
        {
            Level++;
            int n = Level;

            double MaxHPAtLevelUp = (Math.Pow(n, 2) / 2) + 10;
            double MaxManaAtLevelUp = (Math.Pow(n, 2) / 3) + 5;
            double AttackAtLevelUp = (Math.Pow(n, 2) / 100) + n + 10;
            double DefenseAtLevelUp = (Math.Pow(n, 2) / 100) + n + 5;
            double SpeedAtLevelUp = (Math.Pow(n, 2) / 100) + n + 2;

            MaxHP = (int)Math.Floor(MaxHPAtLevelUp);
            HitPoints = MaxHP;

            MaxMana = (int)Math.Floor(MaxManaAtLevelUp);
            Mana = MaxMana;

            Attack = (int)Math.Ceiling(AttackAtLevelUp);
            Defense = (int)Math.Ceiling(DefenseAtLevelUp);
            Speed = (int)Math.Floor(SpeedAtLevelUp);
        }
        
        protected void LevelDown()
        {
            if (Level != 1)
            {
                Level -= 2;
                LevelUp();

                Experience = (int)Mathf.Floor((_nextLvlUp - 8) / 2);
            }
        }

        protected void SetLevel(int level)
        {
            Level = 1;
            Experience = 0;
            _nextLvlUp = 10;
            
            while (Level != level)
            {
                GainExperience(_nextLvlUp - Experience);
            }
        }
        #endregion
    }
}

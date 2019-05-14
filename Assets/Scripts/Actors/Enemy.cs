using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Actors
{
    [Serializable]
    public class Enemy : Actor
    {
        public Enemy(string name, int level, int xpReward, int maxHP, int attack, int defense, int speed) 
        {
            Name = name;
            Level = level;
            Experience = xpReward;
            MaxHP = maxHP;
            HitPoints = maxHP;
            Attack = attack;
            Defense = defense;
            Speed = speed;
        }

        public int GiveExperience()
        {
            return Experience;
        }
    }
}


﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Actors
{
    [Serializable]
    public class Enemy : Actor
    {
        public Enemy(string name, int level, int xpReward, int hitPoints, int maxHP, int attack, int defense, int speed) 
        {
            Name = name;
            Level = level;
            Experience = xpReward;
            HitPoints = hitPoints;
            MaxHP = maxHP;
            Attack = attack;
            Defense = defense;
            Speed = speed;
        }

        public void ChangeLevel(int level)
        {
            LevelUp(level);
        }
    }
}

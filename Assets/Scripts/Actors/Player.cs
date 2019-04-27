using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Actors
{
    [Serializable]
    public class Player : Actor
    {
        public Player(string name) : base(name)
        {

        }

        public void CheckLevelUp(int gainedExperience)
        {
            GainExperience(gainedExperience);
            int n = (int)Math.Round(Math.Pow(Experience, (double)1 / 3));

            for (int i = n - Level; i <= n; i++)
                LevelUp(i);
        }

        public int ExpNeededToLvlUp()
        {
            int nextMaxXp = (int)Math.Pow(Level + 1, 3);

            return nextMaxXp - Experience;
        }
    }
}


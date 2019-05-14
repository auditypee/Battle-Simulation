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

        public void GainExp(int gainedExperience)
        {
            GainExperience(gainedExperience);
        }

        public void NextLevel()
        {
            GainExperience(NextLvlUp - Experience);
        }

        public void SetLvl(int level)
        {
            SetLevel(level);
        }
    }
}


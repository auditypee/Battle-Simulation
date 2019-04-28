using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Controllers
{
    [Serializable]
    public class HandleTurn
    {
        public string AttackerTag;
        public GameObject Attacker;
        public int AttackerSPD;

        public GameObject Target;

        // TODO: - if more than one hero and want to use combo attacks,
        //         combine turn into one
    }
}


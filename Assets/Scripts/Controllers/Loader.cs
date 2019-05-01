using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Controllers
{
    public class Loader : MonoBehaviour
    {
        public GameObject battleManager;

        private void Awake()
        {
            if (BattleManager.instance == null)
                Instantiate(battleManager);
        }
    }
}

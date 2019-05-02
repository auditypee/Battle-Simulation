using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Menu
{
    public class Loader : MonoBehaviour
    {
        public GameObject gameManager;

        private void Awake()
        {
            if (GameManager.Instance == null)
                Instantiate(gameManager);
        }
    }
}

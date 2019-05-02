using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Buttons
{
    public class StartEncounterButtonHandler : MonoBehaviour
    {
        public delegate void LoadBattle();
        public static event LoadBattle OnClickLoadScene;

        public void OnClick_LoadScene()
        {
            OnClickLoadScene?.Invoke();
        }
    }
}

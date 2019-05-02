using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Actors;
using UnityEngine;

namespace Buttons
{
    public class EnemyPresetButtonHandler : MonoBehaviour
    {
        public delegate void EnemyPreset(Enemy enemy);
        public static event EnemyPreset OnClickPopulate;

        public Enemy Enemy;

        public void OnClick_PopulateEnemyFields()
        {
            OnClickPopulate?.Invoke(Enemy);
        }
    }
}

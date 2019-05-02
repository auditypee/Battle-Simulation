using UnityEngine;
using UnityEngine.UI;
using Actors;

namespace Buttons
{
    public class EnemyPortraitHandler : MonoBehaviour
    {
        public delegate void EnemyDelete(Enemy enemy);
        public static event EnemyDelete OnClickDelete;

        public Enemy Enemy;

        public void OnClick_RemovePortrait()
        {
            OnClickDelete?.Invoke(Enemy);

            Destroy(gameObject);
        }
    }
}

using UnityEngine;

namespace Buttons
{
    public class CreateEnemyButtonHandler : MonoBehaviour
    {
        public delegate void EnemyCreate();
        public static event EnemyCreate OnClickCreate;
        
        public void OnClick_EnemyCreate()
        {
            OnClickCreate?.Invoke();
        }
    }
}

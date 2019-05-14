using UnityEngine;

namespace Buttons
{
    public class ResetCreateEnemyButtonHandler : MonoBehaviour
    {
        public delegate void EnemyCreate();
        public static event EnemyCreate OnClickCreate;
        public static event EnemyCreate OnClickReset;
        
        public void OnClick_ResetFields()
        {
            OnClickReset?.Invoke();
        }

        public void OnClick_EnemyCreate()
        {
            OnClickCreate?.Invoke();
        }
    }
}

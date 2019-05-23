using Controllers;
using UnityEngine;

namespace Buttons
{
    public class AttackOptionButtonHandler : MonoBehaviour
    {
        public delegate void ToggleEnemyPanel();
        public static event ToggleEnemyPanel OnClickToggle;

        public void OnClick_ShowEnemySelection()
        {
            OnClickToggle?.Invoke();
        }
    }
}
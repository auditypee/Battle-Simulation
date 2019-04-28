using UnityEngine;
using Controllers;

namespace Buttons
{
    public class AttackOptionButtonHandler : MonoBehaviour
    {
        public void OnClick_ShowEnemySelection()
        {
            BattleManager.instance.EnemySelectPanel.SetActive(true);
            BattleManager.instance.CreateEnemyButtons();
        }
    }
}

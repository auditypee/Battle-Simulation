using Controllers;
using UnityEngine;

namespace Buttons
{
    public class AttackOptionButtonHandler : MonoBehaviour
    {
        public void OnClick_ShowEnemySelection()
        {
            BattleManager bm = BattleManager.Instance;
            bm.EnemySelectPanel.SetActive(!bm.EnemySelectPanel.activeSelf);
            bm.CreateEnemyButtons();
        }
    }
}
using UnityEngine;
using Controllers;

namespace Buttons
{
    public class AttackOptionButtonHandler : MonoBehaviour
    {
        public void OnClick_ShowEnemySelection()
        {
            BattleManager bm = BattleManager.instance;
            bm.EnemySelectPanel.SetActive(!bm.EnemySelectPanel.activeSelf);
            bm.CreateEnemyButtons();
        }
    }
}

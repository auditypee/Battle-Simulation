using UnityEngine;
using Controllers;

namespace Buttons
{
    public class AttackOptionButtonHandler : MonoBehaviour
    {
        public void OnClick_ShowEnemySelection()
        {
            BattleManager bm = GameObject.Find("BattleManager").GetComponent<BattleManager>();
            bm.EnemySelectPanel.SetActive(!bm.EnemySelectPanel.activeSelf);
            bm.CreateEnemyButtons();
        }
    }
}

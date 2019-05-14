using Controllers;
using UnityEngine;

namespace Buttons
{
    public class AttackOptionButtonHandler : MonoBehaviour
    {
        public void OnClick_ShowEnemySelection()
        {
            BattleManager.Instance.EnemySelectPanel.SetActive(!BattleManager.Instance.EnemySelectPanel.activeSelf);
        }
    }
}
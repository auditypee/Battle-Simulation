using UnityEngine;
using Controllers;

namespace Buttons
{
    public class AttackOptionButtonHandler : MonoBehaviour
    {
        public void OnClick_ShowEnemySelection()
        {
            BattleManager _bm = GameObject.Find("BattleManager").GetComponent<BattleManager>();
            _bm.EnemySelectPanel.SetActive(true);
        }
    }
}

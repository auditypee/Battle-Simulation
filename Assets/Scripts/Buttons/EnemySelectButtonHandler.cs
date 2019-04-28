using UnityEngine;
using Controllers;

namespace Buttons
{
    public class EnemySelectButtonHandler : MonoBehaviour
    {
        public GameObject EnemyPrefab;

        public void OnClick_TargetSelect()
        {
            BattleManager _bm = GameObject.Find("BattleManager").GetComponent<BattleManager>();
            _bm.EnemySelectPanel.SetActive(false);
            
            // TODO: - will have issues if there is more than one Player
            PlayerController playerController = GameObject.Find("Player").GetComponent<PlayerController>();
            playerController.TargetSelected(EnemyPrefab);
        }

        public void ShowSelector()
        {
            EnemyPrefab.GetComponentInChildren<EnemyController>().Selector.SetActive(true);
        }

        public void HideSelector()
        {
            EnemyPrefab.GetComponentInChildren<EnemyController>().Selector.SetActive(false);
        }
    }

}

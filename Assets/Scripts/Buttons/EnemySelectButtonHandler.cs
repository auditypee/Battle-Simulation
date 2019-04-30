using UnityEngine;
using Controllers;
using UnityEngine.Events;

namespace Buttons
{
    public class EnemySelectButtonHandler : MonoBehaviour
    {
        public delegate void TargetSelected(GameObject target);
        public static event TargetSelected OnClickedTarget;

        public GameObject EnemyPrefab;

        public void OnClick_TargetSelect()
        {
            OnClickedTarget?.Invoke(EnemyPrefab);
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

using UnityEngine;

namespace Buttons
{
    public class LvlUpPlayerButtonHandler : MonoBehaviour
    {
        public delegate void PlayerLvl();
        public static event PlayerLvl OnClickLevelUp;
        public static event PlayerLvl OnClickLevelDown;

        public void OnClick_PlayerLvlUp()
        {
            OnClickLevelUp?.Invoke();
        }

        public void OnClick_PlayerLvlDn()
        {
            OnClickLevelDown?.Invoke();
        }
    }


}
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

namespace Buttons
{
    public class LvlUpPlayerButtonHandler : MonoBehaviour
    {
        public delegate void PlayerLvl();
        public static event PlayerLvl OnClickLevelUp;

        public delegate void PlayerSetLvl(int n);
        public static event PlayerSetLvl OnClickLevelSet;

        public InputField LevelToSet;

        public void OnClick_PlayerLvlUp()
        {
            OnClickLevelUp?.Invoke();
        }
        public void OnClick_PlayerSetLvl()
        {
            if (!string.IsNullOrEmpty(LevelToSet.text))
            {
                int n = int.Parse(LevelToSet.text);

                if (n > 0 && n < 100)
                    OnClickLevelSet?.Invoke(n);
                else
                    Debug.Log("Level should be between 0 and 100 exclusive");
            }

            LevelToSet.text = "";
        }
    }


}
using UnityEngine;
using Actors;
using UnityEngine.UI;

namespace Buttons
{
    public class CreatePlayerButtonHandler : MonoBehaviour
    {
        public delegate void PlayerCreate(Player player);
        public static event PlayerCreate OnClickCreate;

        public InputField PlayerName;

        public void OnClick_PlayerCreate()
        {
            string playerName = PlayerName.text.Trim();

            if (playerName != string.Empty)
            {
                Player player = new Player(playerName);

                OnClickCreate?.Invoke(player);
            }
            else
            {
                Player player = new Player("Venet");

                OnClickCreate?.Invoke(player);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class NameCharacter : MonoBehaviour
    {
        public PlayerStatsManager player;
        public InputField inputField;
        public Text nameButtonText;

        public void NameMyCharacter()
        {
            player.playerName = inputField.text;

            if(player.playerName == "")
            {
                player.playerName = "Nameless";
            }

            nameButtonText.text = player.playerName;
        }
    }
}
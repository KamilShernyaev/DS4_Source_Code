using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class SoulCountBar : MonoBehaviour
    {
        public Text soulCountText;
        public UserData userScoreData;
    
        public void SetSoulCountText(int currentSoulCount)
        {
            soulCountText.text = currentSoulCount.ToString();
            userScoreData.userScore = soulCountText.text;
        }
    }
}
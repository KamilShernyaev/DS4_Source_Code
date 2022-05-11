using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardOpen : MonoBehaviour
{
   [SerializeField] private GameObject leaderboard;
   [SerializeField] private GameObject loseWindow;

   private void Start() 
   {
       leaderboard.SetActive(false);
   }

   public void OpenLeaderboard()
   {
       leaderboard.SetActive(true);
       loseWindow.SetActive(false);
   }

   public void CloseLeaderboard()
   {
       leaderboard.SetActive(false);
       loseWindow.SetActive(true);
   }
}

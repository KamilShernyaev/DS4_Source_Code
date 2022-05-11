using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class DisplayHighScore : MonoBehaviour
    {
        private DatabaseAccess databaseAccess;

    // private TextMeshPro highScoreOutPut;

        public Text highScoreOutPut;

        void Start()
        {
            databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();
            //highScoreOutPut = GetComponentInChildren<TextMeshPro>();
            Invoke("DisplayHighScoreInTextMesh", 2f);
        }

        private async void DisplayHighScoreInTextMesh()
        {
            var task = databaseAccess.GetScoresFromDataBase();
            var result = await task;
            var output = "";
            foreach (var score in result)
            {
                output += score.UserName + " Score: " + score.Score + "\n";
            }
            highScoreOutPut.text = output;
        }
    }
}

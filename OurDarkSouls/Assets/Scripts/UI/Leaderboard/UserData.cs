using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "UserData")]
public class UserData : ScriptableObject
{
    public string userName;
    public string userScore;
}

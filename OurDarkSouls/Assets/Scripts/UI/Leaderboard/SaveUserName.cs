using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveUserName : MonoBehaviour
{
    public Text userNameText;
    public UserData userNameData;

    public void AddUserName() 
    {
        userNameData.userName = userNameText.text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class PopupLogout : Popup
{
    public void Logout()
    {
        Hide();
        Backend.BMember.Logout();
        Application.Quit();
    }
}

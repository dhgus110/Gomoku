using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMainExit : Popup
{
    public void Quit()
    {
        Hide();
        Application.Quit();
    }

}

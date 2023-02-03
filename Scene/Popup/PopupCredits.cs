using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PopupCredits : Popup
{
    [SerializeField] Image bg;

    private Animator creditAni;
    private Animator GetCreditAni
    {
        get
        {
            if (creditAni == null)
                creditAni = bg.GetComponent<Animator>();

            return creditAni;
        }
    }

    public void CreditsShow()
    {
        Show();
        GetCreditAni.Play("in");
    }

    public void CreditsHide()
    {
        Hide();
        GetCreditAni.StopPlayback();
    }


}

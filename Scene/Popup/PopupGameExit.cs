using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PopupGameExit : Popup
{
    public void HideAndExit()
    {
        base.Hide();
        GameManager gm = FindObjectOfType<GameManager>();

        //결과창 띄우기
        //마스터가 나감
        if (PhotonNetwork.IsMasterClient)
        {
            if (gm.countTurn > 7)
                gm.M_O_GameResult(2, 1); //m-lose /o-win
            else
                gm.M_O_GameResult(2, 3); //m-lose / o -tie
        }
        //슬레이브가 나감
        else
        {
            if (gm.countTurn > 7)
                gm.O_M_GameResult(1, 2); //m-win /o-lose
            else
                gm.O_M_GameResult(3, 2); //m-tie / o -lose
        }
    }
}

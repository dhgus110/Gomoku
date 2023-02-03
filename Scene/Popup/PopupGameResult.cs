using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


public class PopupGameResult : Popup
{
    [SerializeField] Text TextContent;
    [SerializeField] Text TextTimer;

    BackendGetTable getTable;

    //상대가 나갔을 때 무승부로 처리한다. (나간 상대는 패배처리)
    //포톤 : 방 나가기
    public void Result(string res)
    {
        if (getTable == null)
            getTable = FindObjectOfType<BackendGetTable>();

        int w = getTable.userData.win;
        int l = getTable.userData.lose;
        int t = getTable.userData.tie;

        Show();

        switch (res)
        {
            case "win":
                TextContent.text =
                    " 승리!\n" +
                    "\n" +
                    $"       승 : {w + getTable.userData.inGameWin} + 1\n" +
                    $"       패 : {l}\n" +
                    $"       무 : {t}\n";
                getTable.userData.inGameWin += 1;
                break;
            case "lose":
                TextContent.text =
                    " 패배!\n" +
                    "\n" +
                    $"       승 : {w}\n" +
                    $"       패 : {l + getTable.userData.inGameLose} + 1\n" +
                    $"       무 : {t}\n";
                getTable.userData.inGameLose += 1;
                break;
            case "tie":

                TextContent.text =
                    "상대방이 나갔습니다.\n" +
                    "\n" +
                    $"       승 : {w}\n" +
                    $"       패 : {l}\n" +
                    $"       무 : {t + getTable.userData.inGameTie} + 1\n";
                getTable.userData.inGameTie += 1;
                break;
        }

        StartCoroutine(GameExit());
    }

    //public void GameExit()
    //{
    //    Hide();
    //    Managers.Scene.LoadScene(Define.Scene.Main);
    //}

    IEnumerator GameExit()
    {
        TextTimer.text = "3초 후에 닫힙니다.";
        yield return new WaitForSeconds(1.0f);
        TextTimer.text = "2초 후에 닫힙니다.";
        yield return new WaitForSeconds(1.0f);
        TextTimer.text = "1초 후에 닫힙니다.";
        yield return new WaitForSeconds(1.0f);

        Hide();

        if (PhotonNetwork.IsMasterClient) //마스터
        {
            PhotonNetwork.LoadLevel("Main");
        }
        //else 
        //{
        //    yield return new WaitForSeconds(1.0f);
        //    if (PhotonNetwork.IsMasterClient)
        //        PhotonNetwork.LoadLevel("Main");
        //    else
        //        Managers.Scene.LoadScene(Define.Scene.Main);

        //}

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PopupMatching : Popup
{
    [SerializeField] private Text text;
    public void CancelMatching()
    {
        text.text = "매칭에 실패했습니다." +
            "3초 후에 닫힙니다.";
        Show();
        StartCoroutine(MatchingExit());

    }
    IEnumerator MatchingExit()
    {
        yield return new WaitForSeconds(3.0f);
        Hide();
    }

}

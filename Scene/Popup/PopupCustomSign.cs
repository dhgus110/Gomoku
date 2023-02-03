using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupCustomSign : Popup
{
    [SerializeField] private Text text;

    public void ShowSignupSuccess()
    {
        Show();
        text.text = "회원가입에 성공했습니다. 로그인 버튼을 눌러주십시오.";
    }

    public void ShowDuplicatedCustomId()
    {
        Show();
        text.text = "중복된 아이디입니다.";
    }

    public void ShowFailedSigup(string error)
    {
        Show();
        text.text = $"error : {error}";
    }

    public void ShowBadLogin(string error)
    {
        Show();
        text.text = $"error : {error}";
    }

    //public void ShowFailedLogin(string error)
    //{
    //    Show();
    //    text.text = $"error : {error}";
    //}
}

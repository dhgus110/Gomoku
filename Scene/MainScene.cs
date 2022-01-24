using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainScene : BaseScene
{
    static BackendGetTable s_instance;
    static BackendGetTable GetTale{ get { TableInit(); return s_instance; } }

    [Header("MainWindow")]
    [SerializeField] private RectTransform UserStateWindow;
    [SerializeField] private RectTransform GameJoinWindow;
    [SerializeField] private RectTransform GameGuideWindow;

    [Header("UserState")]
    [SerializeField] private Text NickName;
    [SerializeField] private Text Win;
    [SerializeField] private Text Lose;
    [SerializeField] private Text Tie;
    [SerializeField] private Text Rate;

    private void Start()
    {
        TableInit();
        UserDataWindow();
    }
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Main;
        
    }

    static void TableInit() // 유저 DB정보 싱글톤 할당.
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@UserTableInformation");
            if (go == null)
            {
                go = new GameObject { name = "@UserTableInformation" };
                go.AddComponent<BackendGetTable>();
            }
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<BackendGetTable>();
        }
    }

    //유저 데이터 
    public void UserDataWindow()
    {
        int w = s_instance.userData.win;
        int l = s_instance.userData.lose;
        int t = s_instance.userData.tie;
        Win.text = w.ToString();
        Lose.text = l.ToString();
        Tie.text = t.ToString();
        NickName.text = s_instance.userData.userNickname;
        float rate =0;
        if (w == 0)
            rate = 0;
        else
            rate = w / (w + l + t);

        Rate.text = string.Format("{0:0.00}", rate)+" %";
    }


    public void OnClickUserState()
    {
        UserStateWindow.DOAnchorPosX(0, 0.2f);
        GameJoinWindow.DOAnchorPosX(1080f, 0f);
        GameGuideWindow.DOAnchorPosX(1080f, 0f);
    }

    public void OnClickGameJoin()
    {
        UserStateWindow.DOAnchorPosX(1080f, 0f);
        GameJoinWindow.DOAnchorPosX(0, 0.2f);
        GameGuideWindow.DOAnchorPosX(1080f, 0f);
    }

    public void OnClickGameInfo()
    {
        UserStateWindow.DOAnchorPosX(1080f, 0f);
        GameJoinWindow.DOAnchorPosX(1080f, 0f);
        GameGuideWindow.DOAnchorPosX(0, 0.2f);
    }





    public override void Clear()
    {
        Debug.Log("Main Scene Clear");
    }

}

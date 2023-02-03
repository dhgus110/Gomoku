using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Photon.Pun;

public class MainScene : BaseScene
{
    BackendGetTable _getTable;
    PhotonManager _photon;

    [Header("MainWindow")]
    [SerializeField] private RectTransform UserStateWindow;
    [SerializeField] private RectTransform RandomMatchingWindow;
    [SerializeField] private RectTransform GameGuideWindow;
    [SerializeField] private RectTransform GameSettingWindow;
    [SerializeField] private GameObject EmoticonWindow; 

    [Header("UserState")]
    [SerializeField] private Text NickName;
    [SerializeField] private Text Win;
    [SerializeField] private Text Lose;
    [SerializeField] private Text Tie;
    [SerializeField] private Text Rate;

    [Header("MatchingWindow")]
    [SerializeField] private GameObject MatchingStatePanel;
    [SerializeField] private GameObject MatchingButton;
    [SerializeField] private GameObject LeftMatchingButton;
    //[SerializeField] private RectTransform MatchingContent;
    [SerializeField] private Animator MatchingAnim;
    [SerializeField] private Text InLobbyCountPlayerText;
    [SerializeField] private Text MatchingSecondText;


    [Header("GuideWindow")]
    [SerializeField] private RectTransform GuidePage1;
    [SerializeField] private RectTransform GuidePage2;
    [SerializeField] private RectTransform GuidePage3;
    [SerializeField] private RectTransform GuidePage4;
                                           
    [Header("EmoticonWindow")]
    [SerializeField] private RectTransform EmoticonListPanel;

    [Header("Popup")]
    private RectTransform MainCanvas;
    public PopupLogout PopupLogout;
    public PopupMainExit PopupMainExit;
    public PopupCredits PopupCredits;
    public PopupMatching PopupMatching;


    private bool _bLeftMatching;    //매칭하는중인지?

    void Awake()
    {
        TableInit();
        PhotonInit();

    }

#if UNITY_ANDROID  
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PopupMainExit.Show();

    }
#endif

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Main;

        UserDataWindow();
        StartCoroutine(InLobbyCountPlayer());
        PopupInit();
    }

    #region USER_DATA
    // 유저 DB정보 싱글톤 할당.
    void TableInit() 
    {
      
        if(_getTable == null)
        {
            _getTable= FindObjectOfType<BackendGetTable>();
        }
    }

    //유저 데이터 
    public void UserDataWindow()
    {
        int w = _getTable.userData.win + _getTable.userData.inGameWin;
        int l = _getTable.userData.lose + _getTable.userData.inGameLose;
        int t = _getTable.userData.tie + _getTable.userData.inGameTie;
        Win.text = w.ToString();
        Lose.text = l.ToString();
        Tie.text = t.ToString();
        NickName.text = _getTable.userData.userNickname;

        float rate =0;
        if (w == 0)
            rate = 0;   
        else
            rate = (float)w / (w + l + t) * 100;

        Rate.text = string.Format("{0:0.00}", rate)+" %";
    }
    #endregion

    #region Photon
    void PhotonInit()
    {
        if(_photon == null)
        {
            _photon = FindObjectOfType<PhotonManager>().Photon;
        }
    }
    #endregion

    void PopupInit()
    {
        MainCanvas = GameObject.Find("MainCanvas").GetComponent<RectTransform>();

        GameObject go = Resources.Load<GameObject>("Prefabs/Popup/Popup_Logout");
        PopupLogout = Instantiate(go, MainCanvas).GetComponent<PopupLogout>();
        PopupLogout.gameObject.SetActive(false);

        GameObject go2 = Resources.Load<GameObject>("Prefabs/Popup/Popup_Main_Exit");
        PopupMainExit = Instantiate(go2, MainCanvas).GetComponent<PopupMainExit>();
        PopupMainExit.gameObject.SetActive(false);

        GameObject go3 = Resources.Load<GameObject>("Prefabs/Popup/Popup_Credits");
        PopupCredits = Instantiate(go3, MainCanvas).GetComponent<PopupCredits>();
        PopupCredits.gameObject.SetActive(false);


        GameObject go4 = Resources.Load<GameObject>("Prefabs/Popup/Popup_MatchingCancel");
        PopupMatching = Instantiate(go4, MainCanvas).GetComponent<PopupMatching>();
        PopupMatching.gameObject.SetActive(false);
    }

    #region UI_CALLBACK
    //------------Main Buttons------------
    public void OnClickUserState()
    {
        UserStateWindow.DOAnchorPosX(0, 0.2f);
        RandomMatchingWindow.DOAnchorPosX(1080f, 0f);
        GameGuideWindow.DOAnchorPosX(1080f, 0f);
        GameSettingWindow.DOAnchorPosX(1080f, 0f);
        ClearGuideWindow();

    }

    public void OnClickGameJoin()
    {
        UserStateWindow.DOAnchorPosX(1080f, 0f);
        RandomMatchingWindow.DOAnchorPosX(0, 0.2f);
        GameGuideWindow.DOAnchorPosX(1080f, 0f);
        GameSettingWindow.DOAnchorPosX(1080f, 0f);
        ClearGuideWindow();
    }

    public void OnClickGameInfo()
    {
        UserStateWindow.DOAnchorPosX(1080f, 0f);
        RandomMatchingWindow.DOAnchorPosX(1080f, 0f);
        GameGuideWindow.DOAnchorPosX(0, 0.2f);
        GameSettingWindow.DOAnchorPosX(1080f, 0f);
        ClearGuideWindow();
    }

    public void OnClickGameSetting()
    {
        UserStateWindow.DOAnchorPosX(1080f, 0f);
        RandomMatchingWindow.DOAnchorPosX(1080f, 0f);
        GameGuideWindow.DOAnchorPosX(1080f, 0f);
        GameSettingWindow.DOAnchorPosX(0, 0.2f);
        ClearGuideWindow();
    }

    //------------User State---------------
    public void GoEmoticonWindow()
    {
        EmoticonWindow.SetActive(true);
        EmoticonListPanel.DOAnchorPosX(0, 0);
    }


    //-------------Emoticon------------
    public void ApplyAndExit()
    {
        EmoticonWindow.SetActive(false);
    }

    public void EmoticonPage1()
    {
        EmoticonListPanel.DOAnchorPosX(0, 0.2f);
    }

    public void EmoticonPage2()
    {
        EmoticonListPanel.DOAnchorPosX(-1080f, 0.2f);
    }

    //------------Random Matching------------
    public void RandomMatching()
    {
        if (!PhotonNetwork.InLobby)
            return;

        _photon.PhotonState = Define.Photon.ApplyEmoticon;
        _photon.PhotonState = Define.Photon.JoinRamdomRoom;
        _bLeftMatching = false;

        MatchingStatePanel.SetActive(true);
        MatchingButton.SetActive(false);
        LeftMatchingButton.SetActive(true);

        if (MatchingAnim != null)
            MatchingAnim.Play("Circle");

        StartCoroutine(CancelMatching());
    }

    public void LeftMatching()
    {
        _photon.PhotonState = Define.Photon.LeaveRoom;
        _bLeftMatching = true;

        LeftMatchingButton.SetActive(false);
        MatchingStatePanel.SetActive(false);
        MatchingButton.SetActive(true);

    }

    //--------------Guide ---------------
    private int _guideIndex = 1;
    public void GuideLeftButton()
    {
        if (_guideIndex == 1) return;
        _guideIndex -= 1;

        switch (_guideIndex)
        {
            case 1:
                GuidePage1.DOAnchorPosX(0, 0.2f);
                GuidePage2.DOAnchorPosX(1080, 0.2f);
                GuidePage3.DOAnchorPosX(1080, 0);
                GuidePage4.DOAnchorPosX(1080, 0);
                break;
            case 2:
                GuidePage1.DOAnchorPosX(-1080f,0 );
                GuidePage2.DOAnchorPosX(0, 0.2f);
                GuidePage3.DOAnchorPosX(1080, 0.2f);
                GuidePage4.DOAnchorPosX(1080, 0);
                break;
            case 3:
                GuidePage1.DOAnchorPosX(-1080, 0);
                GuidePage2.DOAnchorPosX(-1080f, 0);
                GuidePage3.DOAnchorPosX(0, 0.2f);
                GuidePage4.DOAnchorPosX(1080, 0.2f);
                break;
        }

    }

    public void GuideRightButton()
    {
        if (_guideIndex == 4) return;
        _guideIndex += 1;

        switch (_guideIndex)
        {
            case 2:
                GuidePage1.DOAnchorPosX(-1080f, 0.2f);
                GuidePage2.DOAnchorPosX(0, 0.2f);
                GuidePage3.DOAnchorPosX(1080, 0);
                GuidePage4.DOAnchorPosX(1080, 0);
                break;
            case 3:
                GuidePage1.DOAnchorPosX(-1080, 0);
                GuidePage2.DOAnchorPosX(-1080f, 0.2f);
                GuidePage3.DOAnchorPosX(0, 0.2f);
                GuidePage4.DOAnchorPosX(1080, 0);
                break;
            case 4:
                GuidePage1.DOAnchorPosX(-1080, 0);
                GuidePage2.DOAnchorPosX(-1080, 0);
                GuidePage3.DOAnchorPosX(-1080f, 0.2f);
                GuidePage4.DOAnchorPosX(0, 0.2f);
                break;
        }
    }

    void ClearGuideWindow()
    {
        GuidePage1.DOAnchorPosX(0, 0);
        GuidePage2.DOAnchorPosX(1080f, 0);
        GuidePage3.DOAnchorPosX(1080f, 0);
        GuidePage4.DOAnchorPosX(1080f, 0);
        _guideIndex = 1;
    }

    //----------------Setting-----------------
    public void Logout()
    {
        PopupLogout.Show();
    }

    public void GameExit()
    {
        PopupMainExit.Show();
    }

    public void Credits()
    {
        PopupCredits.CreditsShow();
    }
    #endregion



    #region COROUTINE
    IEnumerator InLobbyCountPlayer()
    {
        WaitForSeconds ws = new WaitForSeconds(5.0f);
        while (true)
        {
            InLobbyCountPlayerText.text = PhotonNetwork.CountOfPlayers.ToString();
            yield return ws;
        }

    }

    IEnumerator CancelMatching()
    {
        int sec = 1;
        WaitForSeconds ws = new WaitForSeconds(1.0f);

        while (sec < 22)
        {
            yield return ws;
            MatchingSecondText.text = $"{sec} s..";
            sec += 1;
        }
        LeftMatching();
        PopupMatching.CancelMatching();
        MatchingSecondText.text="";
    }

    #endregion
    public override void Clear()
    {
        StopAllCoroutines();
        //Debug.Log("Main Scene Clear");
    }

}

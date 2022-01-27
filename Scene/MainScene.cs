using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainScene : BaseScene
{
    static MainScene _mainScene;
    static MainScene MInstance { get { MainSceneInit(); return _mainScene; } }

    BackendGetTable _getTable;
    public static BackendGetTable GetTale { get {  return MInstance._getTable; } }

    PhotonManager _photon;
    public static PhotonManager GetPhoton { get { return MInstance._photon; } }

    [Header("MainWindow")]
    [SerializeField] private RectTransform UserStateWindow;
    [SerializeField] private RectTransform RandomMatchingWindow;
    [SerializeField] private RectTransform GameGuideWindow;
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
    [SerializeField] private RectTransform MatchingContent;

    [Header("EmoticonWindow")]
    [SerializeField] private RectTransform EmoticonListPanel;


    private bool _bLeftMatching;    //매칭하는중인지?

    void Awake()
    {
        TableInit();
        PhotonInit();
        MainSceneInit();


    }
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Main;

        UserDataWindow();
    }

    #region MAIN_SCENE
    static void MainSceneInit()
    {
        if (_mainScene == null)
        {
            GameObject go = GameObject.Find("@MainScene");
            if(go == null)
            {
                go = new GameObject { name = "@MainScene" };
                go.AddComponent<MainScene>();
            }
            DontDestroyOnLoad(go);
            _mainScene = go.GetComponent<MainScene>();
        }
    }
    #endregion

    #region USER_DATA
    // 유저 DB정보 싱글톤 할당.
    void TableInit() 
    {
        if (_getTable == null)
        {
            GameObject go = GameObject.Find("@UserTableInformation");
            if (go == null)
            {
                go = new GameObject { name = "@UserTableInformation" };
                go.AddComponent<BackendGetTable>();
            }
            DontDestroyOnLoad(go);
            _getTable = go.GetComponent<BackendGetTable>();
        }
    }

    //유저 데이터 
    public void UserDataWindow()
    {
        int w = _getTable.userData.win;
        int l = _getTable.userData.lose;
        int t = _getTable.userData.tie;
        Win.text = w.ToString();
        Lose.text = l.ToString();
        Tie.text = t.ToString();
        NickName.text = _getTable.userData.userNickname;
        float rate =0;
        if (w == 0)
            rate = 0;
        else
            rate = w / (w + l + t) * 100;

        Rate.text = string.Format("{0:0.00}", rate)+" %";
    }
    #endregion

    #region Photon
    void PhotonInit()
    {
        if (_photon == null)
        {
            GameObject go = GameObject.Find("@PhotonManager");
            if (go == null)
            {
                go = new GameObject { name = "@PhotonManager" };
                go.AddComponent<PhotonManager>();
            }
            DontDestroyOnLoad(go);
            _photon = go.GetComponent<PhotonManager>();
        }
    }
    #endregion

    #region UI_CALLBACK
    //------------Main Buttons------------
    public void OnClickUserState()
    {
        UserStateWindow.DOAnchorPosX(0, 0.2f);
        RandomMatchingWindow.DOAnchorPosX(1080f, 0f);
        GameGuideWindow.DOAnchorPosX(1080f, 0f);
    }

    public void OnClickGameJoin()
    {
        UserStateWindow.DOAnchorPosX(1080f, 0f);
        RandomMatchingWindow.DOAnchorPosX(0, 0.2f);
        GameGuideWindow.DOAnchorPosX(1080f, 0f);
    }

    public void OnClickGameInfo()
    {
        UserStateWindow.DOAnchorPosX(1080f, 0f);
        RandomMatchingWindow.DOAnchorPosX(1080f, 0f);
        GameGuideWindow.DOAnchorPosX(0, 0.2f);
    }

    //------------User State---------------
    public void GoEmoticonWindow()
    {
        EmoticonWindow.SetActive(true);
        EmoticonListPanel.DOAnchorPosX(0, 0);
    }

    //------------Random Matching------------
    public void RandomMatching()
    {
        _photon.PhotonState = Define.Photon.JoinRamdomRoom;
        _bLeftMatching = false;

        MatchingStatePanel.SetActive(true);
        MatchingButton.SetActive(false);
        LeftMatchingButton.SetActive(true);

        StartCoroutine(MatchingShake());
    }

    public void LeftMatching()
    {
        _photon.PhotonState = Define.Photon.LeaveRoom;
        _bLeftMatching = true;

        LeftMatchingButton.SetActive(false);
        MatchingStatePanel.SetActive(false);
        MatchingButton.SetActive(true);

        StopCoroutine(MatchingShake());
    }

    //-------------Emoticon------------
    public void ApplyAndExit()
    {
        _photon.PhotonState = Define.Photon.ApplyEmoticon;
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
    #endregion



    #region COROUTINE
    IEnumerator MatchingShake()
    {
        while (true)
        {
            if (_bLeftMatching)
                break;
            //MatchingContent.Rotate(Vector3.forward * 2f);
            MatchingContent.DOShapeCircle(Vector2.up * 100f, -180f, 3f);

            yield return new WaitForSeconds(3f);
        }
        yield return null;
    }

    #endregion
    public override void Clear()
    {
        StopAllCoroutines();
        Debug.Log("Main Scene Clear");
    }

}

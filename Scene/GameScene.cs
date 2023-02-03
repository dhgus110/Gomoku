using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Photon.Pun;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameScene : BaseScene
{
    [Header("GameStartScene")]
    [SerializeField] private Transform GameStartPanel;
    [SerializeField] private RectTransform FirstStartImage;
    [SerializeField] private RectTransform SecondStartImage;


    [Header("UserInfo")]
    [SerializeField] private Image O_ProfileImage;
    [SerializeField] private Text O_NicknameText;
    [SerializeField] private Image ProfileImage;
    [SerializeField] private Text NicknameText;
    [SerializeField] private Text WinText;
    [SerializeField] private Text LoseText;
    [SerializeField] private Text TieText;

    [Header("Emoticon")]
    [SerializeField] private Image O_ShowEmoticon;
    [SerializeField] private Image ShowEmoticon;
    [SerializeField] private Image[] Emoticons;
    [SerializeField] private RectTransform EmoticonPanel;
    [SerializeField] private Image Blocker;
    private string[] S_Emoticon;

    [Header("StopWatch")]
    [SerializeField] private Slider O_TimerSlider;
    [SerializeField] private TextMeshProUGUI O_PocketWatchText;
    [SerializeField] private Slider TimerSlider;
    [SerializeField] private TextMeshProUGUI PocketWatchText;
    private Coroutine Co_Timer;

    [Header("Board")]
    [SerializeField] private Image BoardShield;
    [SerializeField] private GridLayoutGroup BanGrid;

    [Header("Popup")]
    [SerializeField] private RectTransform GameUICanvasRectTransform;
    public PopupGameExit PopupGameExit;
    public PopupGameResult PopupGameResult;

    private BackendGetTable getTable;
    private GameManager GameManager; 

    protected override void Init()
    {
        SceneType = Define.Scene.Game;

        UserDataInit();
        GetEmoticon();
        ProfileInit();
        PopupInit();

        if (GameManager == null)
            GameManager = FindObjectOfType<GameManager>();

    }

    #region INIT
    //void UserDataInit(System.Action action)
    void UserDataInit()
    {
        if (getTable == null)
        {
            getTable = FindObjectOfType<BackendGetTable>();
            if (getTable == null)
            {
                if(PhotonNetwork.IsMasterClient)
                    getTable = FindObjectOfType<BackendGetTable>();
            }
        }

    }

    void GetEmoticon()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        S_Emoticon = cp["MyEmoticons"].ToString().Split('/');
        if (cp["MyEmoticons"] != null)
        {
            for (int i = 0; i < S_Emoticon.Length - 1; i++)
            {
                Emoticons[i].sprite = Resources.Load<Sprite>($"Images/Emoticon/{S_Emoticon[i]}");
            }
        }

    }

    void ProfileInit()
    {
        O_NicknameText.text = getTable.userData.o_userNickname;

        NicknameText.text = getTable.userData.userNickname;
        WinText.text =  (getTable.userData.win + getTable.userData.inGameWin).ToString();
        LoseText.text = (getTable.userData.lose + getTable.userData.inGameLose).ToString();
        TieText.text = (getTable.userData.tie + getTable.userData.inGameTie).ToString();

    }

    void PopupInit()
    {
        GameUICanvasRectTransform = GameObject.Find("GameUICanvas").GetComponent<RectTransform>();

        GameObject go = Resources.Load<GameObject>("Prefabs/Popup/Popup_Game_Exit");
        PopupGameExit = Instantiate(go, GameUICanvasRectTransform).GetComponent<PopupGameExit>();
        PopupGameExit.gameObject.SetActive(false);

        GameObject go2 = Resources.Load<GameObject>("Prefabs/Popup/Popup_Game_Result");
        PopupGameResult = Instantiate(go2, GameUICanvasRectTransform).GetComponent<PopupGameResult>();
        PopupGameResult.gameObject.SetActive(false);

    }
    #endregion

    #region UI_CALLBACK
    public void OnClickedEmoticonOn()
    {
        EmoticonPanel.DOAnchorPosX(-540f, 0.2f);
        Blocker.gameObject.SetActive(true);
    }

    public void OnClickedEmoticonOff()
    {
        EmoticonPanel.DOAnchorPosX(540f, 0f);
        Blocker.gameObject.SetActive(false);

    }

    public void OnClickedSendEmoticon(int _index)
    {
        string emoticon = GetEmoticonName(_index);
        if (emoticon != null)
        {
            // 1 대 1 임티 보내기
            //if(PhotonNetwork.IsMasterClient)
            //{
            //    //로컬 실행
            //    //M_O_RPc
            //}
            //else
            //{
            //    //로컬 실행 
            //    //O_M_Rpc(_index 보내기)
            //}

            //다 대 다 임티 보내기
            //others -> all  
            GameManager.GetComponent<EmoticonManager>().O_A_Push(emoticon, getTable.userData.userNickname);
            //자신은 로컬에서 실행
            ShowMyEmoticon(emoticon); 
        }
        OnClickedEmoticonOff();

    }

    public void GoPopupGameExit()
    {
        PopupGameExit.Show();

    }

    public void ShowGameResult(string res)
    {
        PopupGameResult.Result(res);

    }

    #endregion

    #region PUBLICK_METHOD
    public Coroutine GameStartUi()
    {
        GameStartPanel.gameObject.SetActive(true);
        return StartCoroutine(C_GameStartUi());

    }

    public void UsedBoardShield(bool _use)
    {
        if (_use)
            BoardShield.gameObject.SetActive(true);
        else
            BoardShield.gameObject.SetActive(false);

    }

    public void ShowMyEmoticon(string _str)
    {
        ShowEmoticon.gameObject.SetActive(true);
        ShowEmoticon.sprite = Resources.Load<Sprite>($"Images/Emoticon/{_str}");
        StartCoroutine(C_ShowEmoticonOff(true));
        OnClickedEmoticonOff();

    }

    public void ShowOpponentEmoticon(string _str)
    {
        O_ShowEmoticon.gameObject.SetActive(true);
        O_ShowEmoticon.sprite = Resources.Load<Sprite>($"Images/Emoticon/{_str}");
        StartCoroutine(C_ShowEmoticonOff(false));
        OnClickedEmoticonOff();

    }

    public void StartTimer(int _timeLimit, bool _isMaster)
    {
        if(Co_Timer!=null)
            StopCoroutine(Co_Timer);
        Co_Timer = StartCoroutine(C_StartTimer(_timeLimit, _isMaster));

    }

    public void StopTimer()
    {
        StopCoroutine(Co_Timer);

    }

    public void OnBanStone(int _index, int _what) //_what : 33,44,6 중 하나
    { 
        Transform go = BanGrid.transform.GetChild(_index).GetChild(0);
        go.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/Ban{_what}");
        go.gameObject.SetActive(true);

        if (!PhotonNetwork.IsMasterClient) //흰돌은 클릭 가능
            go.GetComponent<Image>().raycastTarget = false;

    }

    public void OffBanStone(int _index)
    {
        Transform go = BanGrid.transform.GetChild(_index).GetChild(0);
        go.gameObject.SetActive(false);

    }

    #endregion

    #region PRIVATE_METHOD
    string GetEmoticonName(int _index)
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        string[] MyEmoticon = cp["MyEmoticons"].ToString().Split('/');

        if (MyEmoticon.Length > _index)
            return MyEmoticon[_index];
        else
            return null;

    }


    #endregion


    #region COROUTINE
    IEnumerator C_GameStartUi()
    {
        FirstStartImage.DOAnchorPosX(-250f, 0.5f);
        yield return new WaitForSeconds(0.4f);
        SecondStartImage.DOAnchorPosX(250f, 0.3f);
        yield return new WaitForSeconds(0.5f);
        FirstStartImage.DOAnchorPosX(-800f, 0.3f);
        yield return new WaitForSeconds(0.2f);
        SecondStartImage.DOAnchorPosX(-800f, 0.3f);
        yield return new WaitForSeconds(0.3f);
        GameStartPanel.gameObject.SetActive(false);

    }

    IEnumerator C_ShowEmoticonOff(bool _isMine)
    {
        yield return new WaitForSeconds(2.0f);
        if (_isMine)
            ShowEmoticon.gameObject.SetActive(false);
        else
            O_ShowEmoticon.gameObject.SetActive(false);

    }

    IEnumerator C_StartTimer(int _timeLimit,bool _isMaster)
    {

        if (_isMaster)
        {
            float time = 0;
            TimerSlider.maxValue = _timeLimit;
            TimerSlider.value = 0;
            PocketWatchText.text = Mathf.Ceil(time).ToString();
            while (time <= _timeLimit)
            {
                time += Time.deltaTime;
                TimerSlider.value = time;
                PocketWatchText.text= Mathf.Ceil(_timeLimit-time).ToString();
                yield return null;
            }
        }
        else
        {
            float time = _timeLimit;
            O_TimerSlider.maxValue = _timeLimit;
            O_TimerSlider.value = 0;
            O_PocketWatchText.text = Mathf.Ceil(time).ToString();
            while (time >= 0f)
            {
                time -= Time.deltaTime;
                O_TimerSlider.value = 25.0f-time;
                O_PocketWatchText.text = Mathf.Ceil(time).ToString();
                yield return null;
            }
        }

        if (PhotonNetwork.IsMasterClient) //마스터가 시간 초과 - 마스터lose 슬레이브win
            FindObjectOfType<GameManager>().M_O_GameResult(2,1);
        else //슬레이브가 시간 초과 - 마스터win 슬레이브lose
            FindObjectOfType<GameManager>().O_M_GameResult(1,2);



        yield return null;

    }

    public override void Clear()
    {
        StopAllCoroutines();
        Debug.Log("Game Scene Clear");

    }
    #endregion


}

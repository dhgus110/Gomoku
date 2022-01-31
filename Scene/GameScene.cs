using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;



public class GameScene : MonoBehaviour
{
    static GameScene _gameScene;
    public static GameScene GInstance { get { return _gameScene; } }

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

    //[SerializeField] private MainScene mainScene;
    

    private void Awake()
    {
        //Init();
        EmoticonInit();
        ProfileInit();
    }


    #region INIT
    //void Init()
    //{
    //    mainScene = GameObject.FindObjectOfType<MainScene>();
    //}

    void EmoticonInit()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        S_Emoticon = cp["MyEmoticons"].ToString().Split('/');

        for(int i = 0; i< S_Emoticon.Length-1; i++)
        {
            Emoticons[i].sprite = Resources.Load<Sprite>($"Images/Emoticon/{S_Emoticon[i]}");
        }
    }

    void ProfileInit()
    {
        O_NicknameText.text = MainScene.GetPhoton.O_Nickname;

        NicknameText.text = MainScene.GetTale.userData.userNickname;
        WinText.text =  MainScene.GetTale.userData.win.ToString();
        LoseText.text = MainScene.GetTale.userData.lose.ToString();
        TieText.text = MainScene.GetTale.userData.tie.ToString();
    }
    #endregion

    #region UI_CALLBACK
    public void OnClickedEmoticon()
    {
        EmoticonPanel.DOAnchorPosX(-540f, 0.2f);
        Blocker.gameObject.SetActive(true);
    }

    public void OnClickedEmoticonOff()
    {
        EmoticonPanel.DOAnchorPosX(540f, 0f);
        Blocker.gameObject.SetActive(false);

    }

    public void GameExit()
    {
        //게임 나가기
        //팝업창 나오기
        //게임을 나갔을 때 포톤 구동 방식은?
        //게임 결과 창 나오기.
    }
    #endregion

    #region PUBLICK_METHOD
    public void ShowMyEmoticon(string _str)
    {
        ShowEmoticon.gameObject.SetActive(true);
        ShowEmoticon.sprite = Resources.Load<Image>($"Images/Emoticon/{_str}").sprite;
        StartCoroutine(ShowEmoticonOff(true));
        OnClickedEmoticonOff();
    }

    public void ShowOpponentEmoticon(string _str)
    {
        O_ShowEmoticon.gameObject.SetActive(true);
        O_ShowEmoticon.sprite = Resources.Load<Image>($"Images/Emoticon/{_str}").sprite;
        StartCoroutine(ShowEmoticonOff(false));
        OnClickedEmoticonOff();
    }

    #endregion


    #region COROUTINE
    IEnumerator ShowEmoticonOff(bool _isMine)
    {
        yield return new WaitForSeconds(2.0f);
        if (_isMine)
            ShowEmoticon.gameObject.SetActive(false);
        else
            O_ShowEmoticon.gameObject.SetActive(false);
    }
    #endregion


}

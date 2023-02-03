using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.UI;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#elif UNITY_IOS
using UnityEngine.SignInWithApple;
#endif


public class BackendFederationAuth : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private GameObject LoginCanvas;
    [SerializeField] private GameObject LoadingCanvas;

    [Header("Custom Login")]
    [SerializeField] private RectTransform CustomLoginField;
    [SerializeField] private RectTransform CustomLogin;
    [SerializeField] private InputField InputField_Id;
    [SerializeField] private InputField InputField_Pw;


    [Header("Google Login")]
    [SerializeField] private RectTransform GoogleLogin;

    [Header("Apple Login")]
    [SerializeField] private RectTransform AppleLoginObject;

    [Header("Token Drop")]
    public bool deleteToken;

    private void Awake()
    {
        Delete();

#if UNITY_ANDROID
        //오브젝트 활성 비활성.
        GoogleLogin.gameObject.SetActive(true);
        AppleLoginObject.gameObject.SetActive(false);
#elif UNITY_IOS
        GoogleLogin.gameObject.SetActive(false);
        AppleLoginObject.gameObject.SetActive(true);
#endif
    }

    private void Delete()
    {
        if (deleteToken)
            Backend.BMember.RefreshTheBackendToken();
        return;
    }

    void Start()
    {
#if UNITY_ANDROID
        //GPGS 설정
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
            .Builder()
            .RequestServerAuthCode(false) //뒤끝 파이어베이스 등 구글 토큰 사용하려면 flase로.
            .RequestEmail()     //이메일 요청.
            .RequestIdToken()   //토큰 요청.
            .Build();

        //커스텀된 정보로 GPS 초기화.
        PlayGamesPlatform.InitializeInstance(config);   
        PlayGamesPlatform.DebugLogEnabled = false;

        //GPGS 시작.
        PlayGamesPlatform.Activate();

#endif

        BackendReturnObject bro = Backend.BMember.LoginWithTheBackendToken();
        if (bro.IsSuccess())
        {
            Debug.Log("유저 인증 토큰 로그인 성공");
            //Managers.Scene.LoadScene(Define.Scene.Main);
            LoginCanvas.SetActive(false);
            LoadingCanvas.SetActive(true);
        }
        else
        {
            Debug.Log("실패");
        }
    }

#if UNITY_ANDROID

    //구글 로그인.
    public void OnClickGoogleAuth()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated == false) //구글 콘솔 로그인이 되어 있지 않을 때
        {
            Social.localUser.Authenticate(success =>
            {
                if (success == false)
                {
                    Debug.Log("구글 로그인 실패");
                    return;
                }

                // 로그인이 성공되었습니다.
                //Debug.Log("GetIdToken - " + PlayGamesPlatform.Instance.GetIdToken());
                //Debug.Log("Email - " + ((PlayGamesLocalUser)Social.localUser).Email);
                //Debug.Log("GoogleId - " + Social.localUser.id);
                //Debug.Log("UserName - " + Social.localUser.userName);
                //Debug.Log("UserName - " + PlayGamesPlatform.Instance.GetUserDisplayName());
                GPGSLogin();
            });
        }
    }

    //구글 토큰 받아오기.
    private string GetTokens()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated) //구글 로그인이 되어있는 상태라면.
        {
            //유저 토큰 받기 첫번째 방법.
            string _IDToken = PlayGamesPlatform.Instance.GetIdToken();
            //두번째 방법.
            //string _IDToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
            return _IDToken;
        }
        else
        {
            Debug.Log("접속되어 있지 않습니다. 잠시 후 다시 시도하여 주세요.");
            return null;
        }
    }

    //구글 토큰으로 뒤끝 회원가입 및 로그인.
    public void GPGSLogin()
    {
        BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "GPGS로 만든 계정.");
        if (BRO.IsSuccess()) //로그인 성공.
        {
            switch (BRO.GetStatusCode())
            {
                case "200":
                    Debug.Log("이미 회원가입 완료.");
                    break;
                case "201":
                    Debug.Log("신규 회원가입 완료.");
                    gameObject.GetComponent<BackendInsertTable>().InsertUserStateTable();//유저 스탯 테이블 생성.
                    Backend.BMember.CreateNickname(Social.localUser.userName);
                    break;
            }
            //user.UserNickname = Social.localUser.userName;
            LoginCanvas.SetActive(false);
            LoadingCanvas.SetActive(true);
        }
        else //로그인 실패.
        {
            switch (BRO.GetStatusCode()) //실패 사유.
            {
                case "403":
                    Debug.Log("차단 유저입니다. 차단 사유 : " + BRO.GetErrorCode());
                    break;

                default:
                    Debug.Log("서버 공통 에러 발생. " + BRO.GetMessage());
                    break;
            }
        }
    }
//#endif

#elif UNITY_IOS
    public void AppleLogin()
    {
        var siwa = gameObject.GetComponent<SignInWithApple>();
        siwa.Login(OnLogin);
    }

    public void OnLogin(SignInWithApple.CallbackArgs args)
    {
        BackendReturnObject bro = Backend.BMember.AuthorizeFederation(args.userInfo.idToken, FederationType.Apple, "siwa");

        //Debug.Log(args.userInfo.idToken); <- 애플 로그인 토큰.

        if (bro.IsSuccess())
        {
            //성공 처리
            Debug.Log("APPLE 로그인 성공");
            if(bro.GetStatusCode()=="201") //신규회원가입
                gameObject.GetComponent<BackendInsertTable>().InsertUserStateTable();//유저 스탯 테이블 생성.
            LoginCanvas.SetActive(false);
            LoadingCanvas.SetActive(true);
        }
        else
        {
            Debug.LogError("Apple 로그인 실패");
            Debug.LogError(bro.GetErrorCode());
            //실패 처리
        }
    }
#endif

    public void ShowCustomSignUp()
    {
        CustomLogin.gameObject.SetActive(false);
        AppleLoginObject.gameObject.SetActive(false);
        GoogleLogin.gameObject.SetActive(false);
        CustomLoginField.gameObject.SetActive(true);
    }
    
    public void OnClickCustomSignUp()
    {
        BackendReturnObject bro = Backend.BMember.CustomSignUp(InputField_Id.text, InputField_Pw.text);
        if (bro.IsSuccess())
        {
            FindObjectOfType<TitleScene>().PopupCustom.ShowSignupSuccess();
        }
        else if( bro.GetStatusCode() == "409")
        {
            FindObjectOfType<TitleScene>().PopupCustom.ShowDuplicatedCustomId();
        }
        else
        {
            FindObjectOfType<TitleScene>().PopupCustom.ShowFailedSigup(bro.GetErrorCode());
        }
    }

    public void OnClickCutomLogin()
    {
        BackendReturnObject bro = Backend.BMember.CustomLogin(InputField_Id.text, InputField_Pw.text);
        if (bro.IsSuccess())
        {
            Debug.Log("로그인에 성공했습니다");
            if (!gameObject.GetComponent<BackendInsertTable>().IsTable("UserStateTable")) //테이블이 없을 경우 (첫 로그인인 경우)
            {
                gameObject.GetComponent<BackendInsertTable>().InsertUserStateTable();//유저 스탯 테이블 생성.
                int rand = Random.Range(0, 1000);
                Backend.BMember.CreateNickname("오목이야" + rand.ToString());
            }

            LoginCanvas.SetActive(false);
            LoadingCanvas.SetActive(true);
        }
        else
        {
            FindObjectOfType<TitleScene>().PopupCustom.ShowBadLogin(bro.GetMessage());
        }
    }

}



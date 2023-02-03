//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using BackEnd;

////구글 해쉬키 변환기 (삭제예정)
//public class BackendGetGoogleHashKey : MonoBehaviour
//{
//    public InputField Input;

//    private void Start()
//    {
//        if (!Backend.IsInitialized)
//            Backend.Initialize(backendCallback);
//    }

//    void backendCallback(BackendReturnObject bro)
//    {
//        Debug.Log(bro);
//        if(bro.IsSuccess())
//        {
//            Debug.Log("초기화 성공");
//        }
//        else
//        {
//            Debug.Log("초기화 실패");
//        }
//    }

//    public void GetGoogleHashKey()
//    {
//        string googlehash = Backend.Utils.GetGoogleHash();
//        if (!string.IsNullOrEmpty(googlehash))
//        {
//            Debug.Log(googlehash);
//            if (Input != null)
//                Input.text = googlehash;
//        }
//        else
//            Debug.Log("존재하지않음");
//    }
//}

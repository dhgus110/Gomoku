using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

//에디터버전에서는 오류를 리턴합니다. -> GetReturnValuetoJSON()
public class BackendVersion : MonoBehaviour
{
    public string version;

    public void Checked()
    {
        var bro = Backend.Utils.GetLatestVersion();
        version = bro.GetReturnValuetoJSON()["version"].ToString();

        //최신 버전일 경우
        if (version == Application.version)
        {
            return;
        }

        //일시정지
        Time.timeScale = 0;

        //현재 앱의 버전과 버전관리에서 설정한 버전이 맞지 않을 경우
        string forceUpdate = bro.GetReturnValuetoJSON()["type"].ToString();

        if (forceUpdate == "1") //선택 업데이트
        {
            Debug.Log("업데이트를 하시겠습니까? y/n");
        }
        else if (forceUpdate == "2") //강제 업데이트
        {
            Debug.Log("업데이트가 필요합니다. 스토어에서 업데이트를 진행해주시기 바랍니다");

            //해당 앱의 스토어로 가게 해주는 유니티 함수
#if UNITY_ANDROID
        //Application.OpenURL("market://details?id=" + Application.identifier);
#elif UNITY_IOS
        //Application.OpenURL("https://itunes.apple.com/kr/app/apple-store/" + "id1461432877");
#endif
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;


public class BackendGetTable : MonoBehaviour
{ 
    public UserData userData;

    private string StatTableKey;

    private void Awake()
    {
        InitUserStateTable();
        Debug.Log(StatTableKey);

        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        userData.ownerIndate = bro.GetReturnValuetoJSON()["row"]["inDate"].ToString();
#if UNITY_ANDROID
        userData.userNickname = Social.localUser.userName;
        GetVersion();
#else
        userData.userNickname = Backend.UserNickName; //임시 코드
#endif
        
    }

    //유저 스탯 테이블 init.
    public void InitUserStateTable()
    {
        var bro = Backend.GameData.GetMyData("UserStateTable", new Where(), 10);
        string OwnerIndate = "";
        if (bro.IsSuccess() == false)
        {
            // 요청 실패 처리
            Debug.Log(bro);
            return;
        }
        if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
        {
            // 요청이 성공해도 where 조건에 부합하는 데이터가 없을 수 있기 때문에
            // 데이터가 존재하는지 확인
            // 위와 같은 new Where() 조건의 경우 테이블에 row가 하나도 없으면 Count가 0 이하 일 수 있다.
            Debug.Log(bro);
            return;
        }
        //내 테이블의 rowindate 할당.
        for (int i = 0; i < bro.Rows().Count; ++i)
        {
            OwnerIndate = bro.Rows()[i]["inDate"]["S"].ToString();
        }
        string[] select = { "Win","Lose","Tie" };

        // 테이블 내 해당 rowIndate를 지닌 row를 조회
        // select에 존재하는 컬럼만 리턴
        var BRO = Backend.GameData.Get("UserStateTable", OwnerIndate, select); //BackEnd Return 데이터 Get.
        var json = BRO.GetReturnValuetoJSON(); //BackEnd Return 데이터 => JSON 데이터로 변환
        userData = new UserData(json); //UserData 클래스에 할당.
        Debug.Log("유저데이터 테스트 : " + userData.lose);
        StatTableKey = OwnerIndate;
    }

    void GetVersion()
    {
        GameObject go = GameObject.Find("@Managers");
        if(go == null)
        {
            Debug.Log("Not Found @Managers!!, I Can't Get the Version");
            return;
        }

        BackendVersion backendVersion = go.GetComponent<BackendVersion>();
        if (string.IsNullOrEmpty(backendVersion.version))
            Debug.Log("empty!! BackendVersion.version!!!");
        else
            userData.version = backendVersion.version;
    }

}

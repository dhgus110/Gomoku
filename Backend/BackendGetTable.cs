using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

#if UNITY_ANDROID
using GooglePlayGames;
#endif
public class BackendGetTable : Singleton<BackendGetTable>
{ 
    public UserData userData;

    //private string StatTableKey;

    protected override void Init()
    {
        InitUserStateTable();
        //Debug.Log(StatTableKey);

#if UNITY_ANDROID
        userData.userNickname = Backend.UserNickName;
        //GetVersion();
#else
        userData.userNickname = Backend.UserNickName;
#endif
    }

    private void OnApplicationQuit()
    {
        UpdateTable();
    }

    //유저 스탯 테이블 init.
    public void InitUserStateTable()
    { 
        var bro = Backend.GameData.GetMyData("UserStateTable", new Where(), 10);
        if (bro.IsSuccess() == false)
        {
            // 요청 실패 처리
            Debug.Log(bro);
            return;
        }
        if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
        {
            // 요청이 성공해도 where 조건에 부합하는 데이터가 없을 수 있기 때문에 데이터가 존재하는지 확인
            // 위와 같은 new Where() 조건의 경우 테이블에 row가 하나도 없으면 Count가 0 이하 일 수 있다.
            Debug.Log(bro);
            return;
        }

        string[] select = { "Win","Lose","Tie" };

        //Get indate
        string inDate = Backend.GameData.Get("UserStateTable", new Where()).GetInDate();
        //Get ownerIndate
        BackendReturnObject bro2 = Backend.BMember.GetUserInfo();
        string ownerIndate = bro2.GetReturnValuetoJSON()["row"]["inDate"].ToString();

        // 테이블 내 해당 rowIndate를 지닌 row를 조회
        // select에 존재하는 컬럼만 리턴
        var BRO = Backend.GameData.GetV2("UserStateTable", inDate, ownerIndate, select);
        var json = BRO.GetReturnValuetoJSON(); //BackEnd Return 데이터 => JSON 데이터로 변환

        userData = new UserData(json); //UserData 클래스에 할당.
        userData.ownerIndate = ownerIndate;

    }

    //뒤끝 데이터에 유저 데이터 정보 갱신
    void UpdateTable()
    {
        ////UserStateTable의 inDate
        var bro = Backend.GameData.Get("UserStateTable",new Where());
        string inDate = bro.GetInDate();

        Param updateParam = new Param();
        updateParam.AddCalculation("Win", GameInfoOperator.addition, userData.inGameWin);
        updateParam.AddCalculation("Lose", GameInfoOperator.addition, userData.inGameLose);
        updateParam.AddCalculation("Tie", GameInfoOperator.addition, userData.inGameTie);


        Backend.GameData.UpdateWithCalculationV2("UserStateTable", inDate, userData.ownerIndate, updateParam);

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

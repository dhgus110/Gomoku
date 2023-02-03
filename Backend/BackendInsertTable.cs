using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

//신규 가입자 테이블 생성
public class BackendInsertTable : MonoBehaviour
{
    public void InsertUserStateTable()
    {
        Param param = new Param();

        BackendReturnObject BRO = Backend.GameData.Insert("UserStateTable", param);

        if (BRO.IsSuccess())
        {
            Debug.Log("indate : " + BRO.GetInDate());
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "404":
                    Debug.Log("존재하지 않는 tableName인 경우");
                    break;

                case "412":
                    Debug.Log("비활성화 된 tableName인 경우");
                    break;

                case "413":
                    Debug.Log("하나의 row( column들의 집합 )이 400KB를 넘는 경우");
                    break;

                default:
                    Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
                    break;
            }
        }
    }

    //테이블이 있는지 조회
    public bool IsTable(string str)
    {
        var bro = Backend.GameData.GetMyData(str, new Where(), 10);
        if (bro.IsSuccess() == false)
        {
            // 요청 실패 처리
            Debug.Log(bro);
            Debug.Log("잘못된 테이블");

            return false;
        }
        if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
        {
            // 요청이 성공해도 where 조건에 부합하는 데이터가 없을 수 있기 때문에
            // 데이터가 존재하는지 확인
            // 위와 같은 new Where() 조건의 경우 테이블에 row가 하나도 없으면 Count가 0 이하 일 수 있다.
            Debug.Log(bro);
            Debug.Log("데이터가 없는 테이블");
            return false;
        }

        return true;
    }
}

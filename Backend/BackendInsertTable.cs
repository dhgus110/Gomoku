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
}

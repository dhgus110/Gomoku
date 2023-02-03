using UnityEngine;
using System;
using LitJson;

[SerializeField]
[Serializable]
public class UserData
{
    [Header("UserStatTable_column")]
    public int win; //db에 저장되어 있는 데이터
    public int lose;
    public int tie;

    public int inGameWin; //게임 중에 변경 될 데이터
    public int inGameLose;
    public int inGameTie;

    public string ownerIndate;
    public string userNickname;
    public string version;

    public string o_userNickname; //상대방 닉네임

    public UserData(JsonData json) //JSON Data 할당 생성자.  
    {
        this.win = int.Parse(json["row"]["Win"]["N"].ToString());
        this.lose = int.Parse(json["row"]["Lose"]["N"].ToString());
        this.tie = int.Parse(json["row"]["Tie"]["N"].ToString());
        //this.OwnerIndate = json["row"]["owner_inDate"]["S"].ToString();
    }


}

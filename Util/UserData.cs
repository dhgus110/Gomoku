using UnityEngine;
using System;
using LitJson;

[SerializeField]
public class UserData
{
    [Header("UserStatTable_column")]
    public int win;
    public int lose;
    public int tie;
    public string ownerIndate;
    public string userNickname;

    public UserData(JsonData json) //JSON Data 할당 생성자.  
    {
        this.win = int.Parse(json["row"]["Win"]["N"].ToString());
        this.lose = int.Parse(json["row"]["Lose"]["N"].ToString());
        this.tie = int.Parse(json["row"]["Tie"]["N"].ToString());
        //this.OwnerIndate = json["row"]["owner_inDate"]["S"].ToString();
    }


}

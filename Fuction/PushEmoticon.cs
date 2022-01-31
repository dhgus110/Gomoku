using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PushEmoticon : MonoBehaviour
{
    public void Push(int _index)
    {
        if (GetEmoticonName(_index) != null)
        {
            if (PhotonNetwork.IsMasterClient)
                EmoticonManager.EmoticonInstance.Show(GetEmoticonName(_index));
            else
                EmoticonManager.EmoticonInstance.O_M_Push(GetEmoticonName(_index));
        }
        GameScene.GInstance.OnClickedEmoticonOff();
    }

    string GetEmoticonName(int _index)
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        string[] MyEmoticon = cp["MyEmoticons"].ToString().Split('/');

        if (MyEmoticon.Length > _index)
            return MyEmoticon[_index];
        else
            return null;
    }
}

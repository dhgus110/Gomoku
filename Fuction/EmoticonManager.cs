using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class EmoticonManager : MonoBehaviour
{
    static EmoticonManager _emoticonManager;
    public static EmoticonManager EmoticonInstance { get { return _emoticonManager;} }

    [SerializeField] private PhotonView pv;

    private void Start()
    {
        pv = GameObject.FindObjectOfType<PhotonView>();
    }

    //other -> master
    public void O_M_Push(string _str)
    {
        if (!PhotonNetwork.IsMasterClient)
            pv.RPC("RPC_AskMaster", RpcTarget.MasterClient, _str);
    }

    //Only master
    public void Show(string _str)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        GameScene.GInstance.ShowMyEmoticon(_str);
        pv.RPC("RPC_O_Show", RpcTarget.Others, _str);
    }


    [PunRPC]
    public void RPC_O_Show(string _str)
    {
        GameScene.GInstance.ShowOpponentEmoticon(_str);
    }

    [PunRPC]
    public void RPC_AskMaster(string _str)
    {
        Show(_str);
    }


}

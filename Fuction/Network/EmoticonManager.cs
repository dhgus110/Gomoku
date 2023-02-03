using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class EmoticonManager : MonoBehaviour
{
    [SerializeField] private GameScene gameScene;
    [SerializeField] private GameManager GameManager;
    private BackendGetTable getTable;

    private void Start()
    {
        if (gameScene == null)
            gameScene = FindObjectOfType<GameScene>();

        if (GameManager == null)
            GameManager = gameObject.GetComponent<GameManager>();

        if (getTable == null)
            getTable = FindObjectOfType<BackendGetTable>();

    }

    //others -> all
    public void O_A_Push(string _str, string _player)
    {
        GameManager.PV.RPC("RPC_Show", RpcTarget.All, _str, _player);

    }

    [PunRPC]
    public void RPC_Show(string _str,string _player)
    {
        //자기 자신이면 실행 X 
        if (_player == getTable.userData.userNickname)
            return;

        gameScene.ShowOpponentEmoticon(_str);

    }

    ////Only master
    //public void Show(string _str, bool _isMaster)
    //{
    //    if (!PhotonNetwork.IsMasterClient)
    //        return;

    //    if (_isMaster)
    //    {
    //        gameScene.ShowMyEmoticon(_str);
    //        GameManager.PV.RPC("RPC_O_ShowOppnent", RpcTarget.Others, _str);
    //    }
    //    else
    //    {
    //        gameScene.ShowOpponentEmoticon(_str);
    //        GameManager.PV.RPC("RPC_O_Show", RpcTarget.Others, _str);

    //    }

    //}

    //other -> master
    //public void O_M_Push(string _str)
    //{
    //    if (!PhotonNetwork.IsMasterClient)
    //        GameManager.PV.RPC("RPC_AskMaster", RpcTarget.MasterClient, _str);
    //}

    //[PunRPC]
    //public void RPC_O_ShowOppnent(string _str)
    //{
    //    gameScene.ShowOpponentEmoticon(_str);
    //}

    //[PunRPC]
    //public void RPC_O_Show(string _str)
    //{
    //    gameScene.ShowMyEmoticon(_str);
    //}

    //[PunRPC]
    //public void RPC_AskMaster(string _str)
    //{
    //    Show(_str,false);
    //}


}

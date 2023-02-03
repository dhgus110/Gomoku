using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private int MaxTime = 25;
    [SerializeField] private GameScene GameScene;
    [SerializeField] private GameManager GameManager;


    private void Start()
    {
        if (GameScene == null)
            GameScene = FindObjectOfType<GameScene>();

    }

    //Only master
    public void Timer(bool _isMaster)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        GameScene.StartTimer(MaxTime, _isMaster);
        GameManager.PV.RPC("RPC_Timer", RpcTarget.Others, !_isMaster);

    }

    [PunRPC]
    private void RPC_Timer(bool _isMaster)
    {
        GameScene.StartTimer(MaxTime, _isMaster);

    }

}

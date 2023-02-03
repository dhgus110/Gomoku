using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{

    [Header("AboutPhoton")]
    public PhotonView PV;

    [Header("AboutScript")]
    [SerializeField] GameScene gameScene;
    [SerializeField] TimerManager timer;
    BackendGetTable getTable;

    public int countTurn;

    private void Start()
    {
        if (gameScene == null)
            gameScene = FindObjectOfType<GameScene>();

        if (timer == null)
            timer = gameObject.GetComponent<TimerManager>();

        if (getTable == null)
            getTable = FindObjectOfType<BackendGetTable>();

        PhotonViewInit();

        StartCoroutine(C_GameStart()); //게임 스타트
    }

    //todo 게임에 나갔을 때
    private void OnApplicationQuit()
    {
        getTable.userData.inGameLose += 1;
    }

    #region INITIALIZE
    void PhotonViewInit()
    {
        if (PV == null)
        {
            var go = GameObject.FindObjectOfType<PhotonView>();
            if (go != null)
                PV = go;
            else
                Debug.Log("Not Found PhotonView!");
        }
    }

    #endregion

    #region PUBLIC_METHOD
    public void Turn(bool _isBlack)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        gameScene.UsedBoardShield(!_isBlack); //마스터 - 보드가림막 On/Off
        PV.RPC("RPC_O_BoardShield", RpcTarget.Others, _isBlack); //상대방 - 보드가림막 On/Off
        timer.Timer(_isBlack); //턴 - 타이머

        countTurn++;
    }

    //other -> master
    public void O_M_GameResult(int _master, int _other)
    {
        //other local
        if (_other == 1)
            gameScene.ShowGameResult("win");
        else if (_other == 2)
            gameScene.ShowGameResult("lose");
        else if (_other == 3)
            gameScene.ShowGameResult("tie");

        //rpc
        PV.RPC("RPC_ShowGameResult", RpcTarget.MasterClient, _master);

    }

    //master -> other 
    public void M_O_GameResult(int _master, int _other)
    {
        //master local
        if (_master == 1)
            gameScene.ShowGameResult("win");
        else if (_master == 2)
            gameScene.ShowGameResult("lose");
        else if (_master == 3)
            gameScene.ShowGameResult("tie");

        //rpc
        PV.RPC("RPC_ShowGameResult", RpcTarget.Others, _other);

    }

    #endregion


    #region RPC
    [PunRPC]
    public void RPC_O_BoardShield(bool _used)
    {
        gameScene.UsedBoardShield(_used);
        countTurn++;
    }

    [PunRPC]
    public void RPC_ShowGameResult(int _who)
    {
        if (_who == 1)
            gameScene.ShowGameResult("win");
        else if (_who == 2)
            gameScene.ShowGameResult("lose");
        else if (_who == 3)
            gameScene.ShowGameResult("tie");
    }

    #endregion

    #region PUNCALLBACKS
    //방장이 바꼈을 때
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //Debug.Log("마스터 바뀜 : " + newMasterClient.NickName);
    }

    //상대방이 게임에서 나갔을 때
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //Debug.Log("상대방 나감 : " + otherPlayer.NickName);

        //룸안에 있고, 마스터가 나갔을 때
        if (PhotonNetwork.InRoom && otherPlayer.IsMasterClient)
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);

        if (countTurn > 8) //8 수 이상일 시
            gameScene.ShowGameResult("win");
        else
            gameScene.ShowGameResult("tie");

    }
    #endregion

    #region COROUTINE
    IEnumerator C_GameStart()
    {
        yield return gameScene.GameStartUi();
        Turn(true);
    }
    #endregion
}

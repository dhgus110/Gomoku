using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [Header("AboutPhoton")]
    public PhotonView PV;
    //private readonly string version;

    [Header("AboutOpponent")]
    public string O_Nickname;

    private Define.Photon _photon = Define.Photon.None;
    public Define.Photon PhotonState
    {
        get { return _photon; }
        set
        {
            _photon = value;
            switch (_photon)
            {
                case Define.Photon.Connected:
                    PhotonNetwork.ConnectUsingSettings();
                    break;
                case Define.Photon.JoinRamdomRoom:
                    PhotonNetwork.JoinRandomRoom();
                    break;
                case Define.Photon.LeaveRoom:
                    PhotonNetwork.LeaveRoom();
                    break;
                case Define.Photon.ApplyEmoticon:
                    SetListEmoticon();
                    break;

            }
        }
    }

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = MainScene.GetTale.userData.userNickname;
        Debug.Log("포톤 서버 통신 횟수 : " + PhotonNetwork.SendRate);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("conneted master!");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(cause);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            SetOpponentInfo();
            //PV.RPC("EnterGame", RpcTarget.MasterClient);
            EnterGame();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed()");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 }) ;
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom()");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed() :" + message);
    }



    //[PunRPC]
    public void EnterGame()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        PhotonNetwork.LoadLevel("Game");
        Debug.Log("-----Game Start!!------");
    }

    void SetListEmoticon()
    {
        GameObject go = GameObject.Find("MyEmoticon"); //슬롯이 모여있는 오브젝트.
        if (go != null)
        {
            int emoticonCnt = go.transform.childCount;
            string MyEmoticons = "";

            for (int i = 0; i < emoticonCnt; i++)
            {
                if (go.transform.GetChild(i).childCount > 0) //슬롯에 자식오브젝트(이모티콘)이 있으면
                    MyEmoticons +=(go.transform.GetChild(i).GetChild(0).name) +'/'; //슬롯의 첫번째 자식오브젝트(이모티콘) 이름을 저장
            }

            PhotonNetwork.SetPlayerCustomProperties(new Hashtable { { "MyEmoticons", MyEmoticons } });
        }
    }

    void SetOpponentInfo()
    {
        string myNickname = PhotonNetwork.NickName;
        if (PhotonNetwork.InRoom)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (myNickname != PhotonNetwork.PlayerList[i].NickName)
                    O_Nickname = PhotonNetwork.PlayerList[i].NickName;
            }
        }
    }


    [SerializeField] private string[] MyEmoticon;

    [ContextMenu("프로퍼티정보")]
    void PropertiesInfo()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        Debug.Log(cp["MyEmoticons"]);
        MyEmoticon = cp["MyEmoticons"].ToString().Split('/');
    }

    [ContextMenu("방 정보")]
    void RoomInfo()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            Debug.Log("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            Debug.Log("현재 방 최대 인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            Debug.Log(playerStr);
        }
        else
        {
            Debug.Log("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            Debug.Log("방 개수 : " + PhotonNetwork.CountOfRooms);
            Debug.Log("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            Debug.Log("로비? : " + PhotonNetwork.InLobby);
            Debug.Log("연결됨? : " + PhotonNetwork.IsConnected);
        }
    }


}

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

    BackendGetTable getTable;

    [Header("AboutEmoticon")]
    [SerializeField] RectTransform MyEmoticonBasket;

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

    [SerializeField] private PhotonManager _photonManager;
    public PhotonManager Photon
    {
        get
        {
            if (_photonManager == null)
                _photonManager = FindObjectOfType<PhotonManager>();
            return _photonManager;
        }
    }


    void Start()
    {
        if (getTable == null)
            getTable = FindObjectOfType<BackendGetTable>();

        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
 
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = getTable.userData.userNickname;
        Debug.Log("포톤 서버 통신 횟수 : " + PhotonNetwork.SendRate);
        PhotonState = Define.Photon.Connected;
 

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("conneted master!");
        PhotonNetwork.JoinLobby(); //auto join lobby
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log(cause);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            SetOpponentInfo();
            PV.RPC("EnterGame", RpcTarget.MasterClient);
            FindObjectOfType<MainScene>().Clear();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //if (!PhotonNetwork.IsConnected) //서버 연결 오류 시 재접속후 방 만들기
        //{
        //    Debug.Log("OnJoinRandomFailed, rejoin server");
        //    ActionConnect(() => PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 }));
        //}
        //else
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    //void ActionConnect(System.Action action)
    //{
    //     PhotonNetwork.ConnectUsingSettings();

    //}

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom()");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed() :" + message);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom()");
    }

    [PunRPC]
    public void EnterGame()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        SetOpponentInfo();
        FindObjectOfType<MainScene>().Clear();
        PhotonNetwork.LoadLevel("Game");
        Debug.Log("-----Game Start!!------");
    }


    void SetListEmoticon()
    {
        //슬롯이 모여있는 오브젝트.

        if (MyEmoticonBasket != null)
        {
            int emoticonCnt = MyEmoticonBasket.transform.childCount;
            string MyEmoticons = "";

            for (int i = 0; i < emoticonCnt; i++)
            {
                if (MyEmoticonBasket.transform.GetChild(i).childCount > 0) //슬롯에 자식오브젝트(이모티콘)이 있으면
                    MyEmoticons +=(MyEmoticonBasket.transform.GetChild(i).GetChild(0).name) +'/'; //슬롯의 첫번째 자식오브젝트(이모티콘) 이름을 저장
            }

            PhotonNetwork.SetPlayerCustomProperties(new Hashtable { { "MyEmoticons", MyEmoticons } });

            PropertiesInfo();

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
                    getTable.userData.o_userNickname = PhotonNetwork.PlayerList[i].NickName;
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
            Debug.Log("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            Debug.Log("방 개수 : " + PhotonNetwork.CountOfRooms);

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

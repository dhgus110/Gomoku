using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class BoardManager : MonoBehaviour
{
    static BoardManager _boardManager;
    public static BoardManager Board { get { return _boardManager; } }

    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private PhotonView pv;

    private void Start()
    {
        pv = GameObject.FindObjectOfType<PhotonView>();
    }

    // other -> master
    public void O_M_Putdown(int _index, bool _isWhite)
    {
        if (!PhotonNetwork.IsMasterClient)
            pv.RPC("RPC_ApplyPutdown", RpcTarget.MasterClient, _index, _isWhite);
    }

    
    public void ApplyPutDown(int _index, bool _isWhite)
    {
        int cnt = _index / 15;
        Transform tr = _boardManager.gridLayoutGroup.transform.GetChild(_index + cnt).GetChild(0).GetChild(0);
        if (tr != null)
        {
            tr.gameObject.SetActive(true);
            if (_isWhite)
                tr.GetComponent<Image>().color = Color.white;
        }
    }
    
    [PunRPC]
    public void RPC_ApplyPutdown(int _index, bool _isWhite)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        ApplyPutDown(_index, _isWhite);
    }
}

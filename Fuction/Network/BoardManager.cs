using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class BoardManager : MonoBehaviour
{
    [SerializeField] private Transform ObjectBoard;
    [SerializeField] private GridLayoutGroup Grid;
    [SerializeField] private RectTransform LastStoneMark;

    [SerializeField] private GameScene GameScene;
    [SerializeField] private GameManager GameManager;
    [SerializeField] private RuleManager Rule;


    private void Start()
    {
        if (GameScene == null)
            GameScene = FindObjectOfType<GameScene>();

        if (GameManager == null)
            GameManager = gameObject.GetComponent<GameManager>();

    }

    //---------- Stone --------------
    // other -> master
    public void O_M_Putdown(int _index)
    {
        if (!PhotonNetwork.IsMasterClient)
            GameManager.PV.RPC("RPC_AskMaster", RpcTarget.MasterClient, _index);

    }
    
    //Only Master
    public void ApplyPutDown(int _index, bool _isBlack)
    {
        StoneActivation(_index, _isBlack);
        Rule.SetStone(_index, _isBlack); //Rule -> master만 관여함 
        GameManager.PV.RPC("RPC_O_ApplyPutDown", RpcTarget.Others, _index, _isBlack);
        GameManager.Turn(!_isBlack);

    }

    [PunRPC]
    public void RPC_O_ApplyPutDown(int _index, bool _isBlack)
    {
        StoneActivation(_index, _isBlack);

    }

    [PunRPC]
    public void RPC_AskMaster(int _index)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        ApplyPutDown(_index, false);

    }
    

    void StoneActivation(int _index, bool _isBlack)
    {
        int cnt = _index / 15;
        Transform tr = Grid.transform.GetChild(_index + cnt).GetChild(0).GetChild(0);
        if (tr != null)
            tr.gameObject.SetActive(true);
        tr.SetParent(ObjectBoard);
        tr.SetAsLastSibling();
        if (!_isBlack)
            tr.GetComponent<Image>().color = Color.white;

        if (LastStoneMark.gameObject.activeSelf == false)
            LastStoneMark.gameObject.SetActive(true);
        LastStoneMark.position = tr.GetComponent<RectTransform>().position;

    }

    //---------- BanStone ---------
    //Only Master
    public void BanStone(int _index, int _what, bool _isActive)
    {
        //Local
        if (_isActive)
            GameScene.OnBanStone(_index, _what);
        else
            GameScene.OffBanStone(_index);

        //Rpc
        GameManager.PV.RPC("O_BanStone", RpcTarget.Others, _index, _what, _isActive);

    }

    [PunRPC]
    private void O_BanStone(int _index, int _what, bool _isActive)
    {
        if (_isActive)
            GameScene.OnBanStone(_index, _what);
        else
            GameScene.OffBanStone(_index);

    }

}

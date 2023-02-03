using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PutDownStone : MonoBehaviour
{
    [Header("AboutUI")]
    [SerializeField] private Transform ObjectBoard; //캐싱 필요
    private Transform PrevObject; //이전에 눌린 오브젝트
    private Transform PrevParent; //이전 부모
    private Transform curObject; //현재 오브젝트

    private Color Yellow = new Color(1f, 1f, 0, 0.5f); //반투명 노랑 
    private Color Blind = new Color(1f, 1f, 0, 0f); //안보임

    [SerializeField] BoardManager Board;

    private void Start()
    {
        if (Board == null)
            Board = gameObject.GetComponent<BoardManager>();

    }

    #region UI_Listener
    public void PutDown()
    {
        curObject = EventSystem.current.currentSelectedGameObject.transform;

        if (PrevObject != curObject) //처음 눌렸을 때, 다른 버튼 눌렸을 때,
        {
            if(PrevObject != null) //이전 오브젝트 원래위치로, 색상 없애기.
            {
                PrevObject.SetParent(PrevParent);
                PrevObject.GetComponent<Image>().color = Blind;
                PrevObject.SetAsLastSibling();
            }
            //현재 오브젝트는 컬러 노랑, 부모 위치  최상위로 변경 , 이전오브젝트변수에 현오브젝트 넣어주기
            PrevObject = curObject;
            PrevParent = curObject.parent;

            curObject.SetParent(ObjectBoard);
            curObject.SetAsLastSibling();
            curObject.GetComponent<Image>().color = Yellow;

        }
        else //같은거 눌렀을 때
        {
            curObject.SetParent(PrevParent);
            curObject.GetComponent<Image>().color = Blind;
            PrevObject = null;
            PrevParent = null;

            int index = curObject.GetComponent<CoordinateInfo>().Index;

            if (PhotonNetwork.IsMasterClient)
                Board.ApplyPutDown(index,true);
            else
                Board.O_M_Putdown(index);
        }

    }
    #endregion

}

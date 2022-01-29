using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class PutDownStone : MonoBehaviour
{
    [Header("AboutUI")]
    [SerializeField] private Transform ObjectBoard;
    [SerializeField] private RectTransform mainShell;



    #region UI_Listner
    public void PutDown()
    {
        Transform button = EventSystem.current.currentSelectedGameObject.transform;

        if (PhotonNetwork.IsMasterClient)//마스터 = 흑돌
        {
            GameObject go = Managers.Resource.Instantiate("OmokObject/BlackStone", button);
            go.transform.SetParent(ObjectBoard);
            go.transform.SetAsLastSibling();
        }
        else
        {
            GameObject go = Managers.Resource.Instantiate("OmokObject/WhiteStone", button);
            go.transform.SetParent(ObjectBoard);
            go.transform.SetAsLastSibling();
        }
    }
    #endregion

}

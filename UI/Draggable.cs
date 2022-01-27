using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform Canvas; // ui가 소속되어 있는 최상단 캔버스 transform
    private Transform PreviousParent; //해당 오브젝트가 직전에 소속되어 있었던 부모 transform
    private RectTransform Rect; //ui 위치 제어를 위한 recttransform
    private CanvasGroup CG; //ui의 알파값과 상호작용 제어 

    private void Awake()
    {
        Canvas = FindObjectOfType<Canvas>().transform;
        Rect = GetComponent<RectTransform>();
        CG = GetComponent<CanvasGroup>();

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        PreviousParent = transform.parent; //드래그 직전에 소속되어 있던 부모 트랜스폼 정보 

        //현재 드래그중인 유아이가 화면의 최상단에 출력되도록
        transform.SetParent(Canvas);  // 부모 오브젝트를 Canvas로 설정
        transform.SetAsLastSibling(); //가장 앞에 보이도록 마지막 자식으로 설정

        //드래그가 가능한 오브젝트를 CanvasGroup으로 통제
        CG.alpha = 0.6f;
        CG.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == Canvas)
        {
            transform.SetParent(PreviousParent);
            Rect.position = PreviousParent.GetComponent<RectTransform>().position;
        }

        CG.alpha = 1.0f;
        CG.blocksRaycasts = true; //광선 출동 처리가 되도록
    }
}

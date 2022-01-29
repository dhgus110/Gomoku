using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class Droppable : MonoBehaviour ,IPointerEnterHandler,IPointerExitHandler ,IPointerDownHandler, IDropHandler
{
    private Image Image;
    private RectTransform Rect;

    void Awake()
    {
        Image = GetComponent<Image>();
        Rect = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Image.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Image.color = Color.white;
    }

    public void OnDrop(PointerEventData eventData)
    {
        //pointerDrag는 현재 드래그하고 있는 대상
        if (eventData.pointerDrag != null)
        {
            //현재 마우스 위치에 있는 오브젝트가 MyEmoticonSlot이 아니라면 오브젝트 삭제
            Debug.Log("---" + eventData.pointerEnter.name);
            GameObject go = eventData.pointerEnter;
            if (go.name != "MyEmoticonSlot")
            {
                Destroy(go);
            }

            GameObject newGo = Managers.Resource.Instantiate($"Emoticon/{eventData.pointerDrag.gameObject.name}", transform);
            //(Clone)삭제
            string str = newGo.name;
            str = str.Replace("(Clone)", "");
            newGo.name = str;

            //eventData.pointerDrag.GetComponent<RectTransform>().position = Rect.position;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {   
    }
}

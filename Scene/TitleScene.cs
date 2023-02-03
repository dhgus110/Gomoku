using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    private RectTransform TitleCanvasRectTransform;
    public PopupCustomSign PopupCustom;

    protected override void Init()
    {
        base.Init();
        TitleCanvasRectTransform = GameObject.Find("LoginCanvas").GetComponent<RectTransform>();

        GameObject go = Resources.Load<GameObject>("Prefabs/Popup/Popup_CustomSign");
        PopupCustom = Instantiate(go, TitleCanvasRectTransform).GetComponent<PopupCustomSign>();
        PopupCustom.gameObject.SetActive(false);


    }

    public override void Clear()
    {
        Debug.Log("Game Scene Clear");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GamesceneUI : MonoBehaviour
{
    [SerializeField] private RectTransform mainShell;

    [ContextMenu("GetScriptFromButton")]
    private void GetScript()
    {
        int size = mainShell.childCount;
        for(int i = 0; i <size; i++)
        {
           if( mainShell.GetChild(i).childCount > 0) 
            {
                GameObject go = mainShell.GetChild(i).GetChild(0).gameObject;
                go.AddComponent<CoordinateInfo>();
            }
        }
    }

    [ContextMenu("CoordinateProperty")]
    private void cp()
    {
        int size = mainShell.childCount;
        int cnt = 0;
        for (int i = 0; i < size; i++)
        {
            if (mainShell.GetChild(i).childCount > 0)
            {
                GameObject go = mainShell.GetChild(i).GetChild(0).gameObject;
                CoordinateInfo ci = go.GetComponent<CoordinateInfo>();
                ci.Index = cnt++;
            }
        }
    }


    [ContextMenu("Instantiate_Boarder_Top")]
    private void Boarder1()
    {
        for (int i = 0; i < 15; i++)
        {
            GameObject go1 = Managers.Resource.Instantiate("OmokObject/ObjectBorderShell", mainShell);
            go1.name = "ObjectBorderShell";
        }
        GameObject go2 = Managers.Resource.Instantiate("OmokObject/ObjectBorderShell(NoBtn)", mainShell);
        go2.name = "ObjectBorderShell";

    }

    [ContextMenu("Instantiate_Boarder_Bottom")]
    private void Boarder2()
    {
        for (int i = 0; i < 16; i++)
        {
            GameObject go2 = Managers.Resource.Instantiate("OmokObject/ObjectBorderShell(NoBtn)", mainShell);
            go2.name = "ObjectBorderShell";
        }
    }

    [ContextMenu("Instantiate_OneLine")]
    private void OneLine()
    {

        GameObject go1 = Managers.Resource.Instantiate("OmokObject/ObjectBorderShell", mainShell);
        go1.name = "ObjectBorderShell";
        for (int i = 0; i < 14; i++)
        {
            GameObject go3 = Managers.Resource.Instantiate("OmokObject/ObjectShell", mainShell);
            go3.name = "ObjectShell";
        }
        GameObject go2 = Managers.Resource.Instantiate("OmokObject/ObjectBorderShell(NoBtn)", mainShell);
        go2.name = "ObjectBorderShell";
    }

    [ContextMenu("Instantiate_14Line")]
    private void FourteenLine()
    {
        for (int j = 0; j < 14; j++)
        {
            GameObject go1 = Managers.Resource.Instantiate("OmokObject/ObjectBorderShell", mainShell);
            go1.name = "ObjectBorderShell";
            for (int i = 0; i < 14; i++)
            {
                GameObject go3 = Managers.Resource.Instantiate("OmokObject/ObjectShell", mainShell);
                go3.name = "ObjectShell";
            }
            GameObject go2 = Managers.Resource.Instantiate("OmokObject/ObjectBorderShell(NoBtn)", mainShell);
            go2.name = "ObjectBorderShell";
        }
    }


}

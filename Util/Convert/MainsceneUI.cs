using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainsceneUI : MonoBehaviour
{
    [SerializeField] private RectTransform mainShell;

    [ContextMenu("sort")]
    private void Gridsort()
    {
        for (int i = 0; i < 225; i++)
        {
            if (i % 15 == 0)
            {
                Color color = new Color();
                color = mainShell.transform.GetChild(i).GetComponent<Image>().color;
                //Debug.Log(color);
                color = new Color(255, 225, 255);
                color.a = 0.1f;

                mainShell.transform.GetChild(i).GetComponent<Image>().color = color;
            }
           
         }
    }

    [ContextMenu("Instantiate_Boarder_Top")]
    private void Boarder1()
    {
        for(int i = 0; i <15; i++)
        {
            GameObject go1= Managers.Resource.Instantiate("OmokObject/ObjectBoardShell", mainShell);
            go1.name = "ObjectBoardShell";
        }
        GameObject go2 = Managers.Resource.Instantiate("OmokObject/ObjectBoardShell(NoBtn)", mainShell);
        go2.name = "ObjectBoardShell";

    }

    [ContextMenu("Instantiate_Boarder_Bottom")]
    private void Boarder2()
    {
        for (int i = 0; i < 16; i++)
        {
            GameObject go2 = Managers.Resource.Instantiate("OmokObject/ObjectBoardShell(NoBtn)", mainShell);
            go2.name = "ObjectBoardShell";
        }
    }

    [ContextMenu("Instantiate_OneLine")]
    private void OneLine()
    {

        GameObject go1 = Managers.Resource.Instantiate("OmokObject/ObjectBoardShell", mainShell);
        go1.name = "ObjectBoardShell";
        for (int i = 0; i < 14; i++)
        {
           GameObject go3 = Managers.Resource.Instantiate("OmokObject/ObjectShell", mainShell);
            go3.name = "ObjectShell";
        }
        GameObject go2 = Managers.Resource.Instantiate("OmokObject/ObjectBoardShell(NoBtn)", mainShell);
        go2.name = "ObjectBoardShell";
    }

    [ContextMenu("Instantiate_14Line")]
    private void FourteenLine()
    {
        for (int j = 0; j < 14; j++)
        {
            GameObject go1 = Managers.Resource.Instantiate("OmokObject/ObjectBoardShell", mainShell);
            go1.name = "ObjectBoardShell";
            for (int i = 0; i < 14; i++)
            {
                GameObject go3 = Managers.Resource.Instantiate("OmokObject/ObjectShell", mainShell);
                go3.name = "ObjectShell";
            }
            GameObject go2 = Managers.Resource.Instantiate("OmokObject/ObjectBoardShell(NoBtn)", mainShell);
            go2.name = "ObjectBoardShell";
        }
    }


}

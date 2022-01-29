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

    [ContextMenu("GetScriptFromButton2")]
    private void GetScript2()
    {
        int size = mainShell.childCount;
        for (int i = 0; i < size; i++)
        {
            if (mainShell.GetChild(i).childCount > 0)
            {
                GameObject go = mainShell.GetChild(i).GetChild(0).gameObject;
                go.AddComponent<ShellButtonInit>();
            }
        }
    }

    [ContextMenu("CoordinateProperty")]
    private void cp()
    {
        int x = 1;
        int y = 1;
        int size = mainShell.childCount;

        for (int i = 0; i < size; i++)
        {
            if (mainShell.GetChild(i).childCount > 0)
            {
                GameObject go = mainShell.GetChild(i).GetChild(0).gameObject;
                CoordinateInfo ci = go.GetComponent<CoordinateInfo>();
                ci.X = x++;
                ci.Y = y;

                if (x == 16)
                {
                    x = 1;
                    y++;
                }
            }
        }
    }

}

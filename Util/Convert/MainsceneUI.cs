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


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShellButtonInit : MonoBehaviour
{
    private void Awake()
    {
        ButtonInit();
    }

    private void ButtonInit()
    {
        PutDownStone pu = GameObject.Find("@RuleManager").GetComponent<PutDownStone>();
        gameObject.GetComponent<Button>().onClick.AddListener(pu.PutDown);

    }
}

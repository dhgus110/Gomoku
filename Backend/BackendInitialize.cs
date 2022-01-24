using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackendInitialize : MonoBehaviour
{
    void Awake()
    {
        var bro = Backend.Initialize(true);
        if (bro.IsSuccess())
        {
            // 초기화 성공 시 로직
            Debug.Log("Backend Init success");
        }
        else
        {
            Debug.LogError("Backend Init Fail");
        }
    }

}

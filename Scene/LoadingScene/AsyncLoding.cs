using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsyncLoding : MonoBehaviour
{
    [SerializeField] private Slider slider; //Progress Bar
    [SerializeField] private int MaxLoadingTime;  //Max Loading Time 

    private float time;

    void Start()
    {
        StartCoroutine(LoadAsynSceneCoroutine());
    }

    IEnumerator LoadAsynSceneCoroutine()
    {
        AsyncOperation operation = Managers.Scene.AsyncScene(Define.Scene.Main);
        operation.allowSceneActivation = false;
        slider.maxValue = MaxLoadingTime;

        while (!operation.isDone)
        {
            time = +Time.time;
            slider.value = time;

            if (time > MaxLoadingTime) 
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

    }

}

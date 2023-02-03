using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsyncLoding : MonoBehaviour
{
    [SerializeField] private Slider Slider; //Progress Bar
    [SerializeField] private float MaxLoadingTime;  //Max Loading Time
    //[SerializeField] private GameObject Manager;
    private BackendVersion BackendVersion;

    private float time;

    void Start()
    {
        StartCoroutine(LoadAsynSceneCoroutine());
    }

    IEnumerator LoadAsynSceneCoroutine()
    {
        //version check
#if UNITY_ANDROID || UNITY_IOS
        //GameObject go = GameObject.Find("@Managers");
        //if (go != null)
        //{
        //    BackendVersion = go.GetComponent<BackendVersion>();
        //    if (BackendVersion == null)
        //    {
        //        go.AddComponent<BackendVersion>();
        //        BackendVersion = go.GetComponent<BackendVersion>();
        //    }
        //    BackendVersion.Checked();
        //}
#endif


        AsyncOperation operation = Managers.Scene.AsyncScene(Define.Scene.Main);
        operation.allowSceneActivation = false;
        Slider.maxValue = MaxLoadingTime;

        while (!operation.isDone)
        {
            time = +Time.time;
            Slider.value = time;

            if (time > MaxLoadingTime) 
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

    }

}

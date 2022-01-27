using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(Define.Scene type)
    {
        SceneManager.LoadScene(GetSceneName(type));
    }

    public AsyncOperation AsyncScene(Define.Scene type)
    {
        return SceneManager.LoadSceneAsync(GetSceneName(type));
    }

    string GetSceneName(Define.Scene type)
    {
        string Name = System.Enum.GetName(typeof(Define.Scene), type);
        return Name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}


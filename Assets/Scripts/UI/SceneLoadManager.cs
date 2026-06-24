using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Title, Map
}
public class SceneLoadManager : Singleton<SceneLoadManager>
{
    // 씬 이름을 타입으로 지정해서 오류 방지
    private Dictionary<SceneType, string> SceneNames = new Dictionary<SceneType, string>()
    {
        {SceneType.Title, "TitleScene"},
        {SceneType.Map, "MapScene"}
    };

    public Func<YieldInstruction> FadeEvent;

    /// <summary>
    /// SceneType에 맞춰 씬을 로드하는 메서드
    /// </summary>
    public void LoadScene(SceneType type)
    {
        if (FadeEvent == null)
            SceneManager.LoadScene(SceneNames[type]);
        else
            LoadSceneWithFade(type);
    }

    private void LoadSceneWithFade(SceneType type)
    {
        StartCoroutine(LoadSceneCoroutine(type));
    }

    private IEnumerator LoadSceneCoroutine(SceneType type)
    {
        yield return FadeEvent();
        SceneManager.LoadScene(SceneNames[type]);
    }
}

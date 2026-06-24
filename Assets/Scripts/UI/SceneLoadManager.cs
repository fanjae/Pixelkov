using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 씬 마다 타입으로 지정
public enum SceneType
{
    Title, Main
}
public class SceneLoadManager : Singleton<SceneLoadManager>
{
    // 타입별로 씬 이름을 묶어서 관리
    private Dictionary<SceneType, string> SceneNames = new Dictionary<SceneType, string>()
    {
        {SceneType.Title, "TitleScene"},
        {SceneType.Main, "MainScene"}
    };
    
    // Fade가 배치된 씬이면 해당 이벤트를 사용해서 DOTween 애니메이션 종료 시점을 알림
    public event Func<YieldInstruction> FadeEvent;

    /// <summary>
    /// SceneType에 맞춰 씬을 로드하는 메서드
    /// </summary>
    public void LoadScene(SceneType type)
    {
        // Fade 기능 존재에 따른 분기
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
        // DOTween 애니메이션 종료까지 기다리기
        yield return FadeEvent?.Invoke();

        SceneManager.LoadScene(SceneNames[type]);
    }
}

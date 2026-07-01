using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 씬 마다 타입으로 지정
public enum SceneType
{
    Title, Main, Clear
}
public class SceneLoadManager : Singleton<SceneLoadManager>
{
    // 타입별로 씬 이름을 묶어서 관리
    private Dictionary<SceneType, string> SceneNames = new Dictionary<SceneType, string>()
    {
        {SceneType.Title, "TitleScene"},
        {SceneType.Main, "MainScene"},
        {SceneType.Clear, "ClearScene" }
    };
    
    // Fade가 배치된 씬이면 해당 이벤트를 사용해서 DOTween 애니메이션 종료 시점을 알림
    public event Func<YieldInstruction> FadeEvent;
    public SceneType CurrentScene { get; private set; } = SceneType.Title;
    public event Action<SceneType> OnSceneChanged; // 씬 전환을 알림
    public bool Loading { get; private set; } = false;  // 씬 전환 코루틴 중복 방지용 변수

    protected override void Awake()
    {
        base.Awake();
        string sceneName = SceneManager.GetActiveScene().name;
        foreach(var kv in SceneNames)
        {
            if(kv.Value == sceneName)
            {
                CurrentScene = kv.Key;
                break;
            }
        }
    }

    /// <summary>
    /// SceneType에 맞춰 씬을 로드하는 메서드
    /// </summary>
    public void LoadScene(SceneType type)
    {
        // Fade 기능 존재에 따른 분기
        if (FadeEvent == null)
        {
            SceneManager.LoadScene(SceneNames[type]);
            OnSceneChanged?.Invoke(type);
        }
        else
            LoadSceneWithFade(type);
    }

    private void LoadSceneWithFade(SceneType type)
    {
        // 씬 전환 중에는 중지
        if (Loading) return;
        StartCoroutine(LoadSceneCoroutine(type));
    }

    private IEnumerator LoadSceneCoroutine(SceneType type)
    {
        Loading = true;

        // DOTween 애니메이션 종료까지 기다리기
        yield return FadeEvent?.Invoke();

        SceneManager.LoadScene(SceneNames[type]);
        OnSceneChanged?.Invoke(type);
        Loading = false;
    }
}

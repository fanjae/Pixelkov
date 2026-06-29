using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Fields")]
    [SerializeField] private AudioSource BGM_Player;
    [SerializeField] private AudioDataBase database;
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        if(database != null)
        {
            database.Init();
        }
    }

    private void Start()
    {
        if(SceneLoadManager.Instance != null)
            SceneLoadManager.Instance.OnSceneChanged += ChangeBGM;
    }
    private void OnDestroy()
    {
        if(SceneLoadManager.Instance != null)
            SceneLoadManager.Instance.OnSceneChanged -= ChangeBGM;
    }

    /// <summary>
    /// sfx타입을 받아 전달 해주는 메서드
    /// </summary>
    public AudioClip GetSFX(SFXType sfxType)
    {
        if (database == null) return null;

        AudioData data = database.GetAudioData(sfxType);
        if (data == null) return null;

        AudioClip nextClip = data.Clip;
        return nextClip;
    }

    /// <summary>
    /// bgm타입을 받아서 재생 해주는 메서드
    /// </summary>
    public void PlayBGM(BGMType bgmType)
    {
        if (BGM_Player == null || database == null) return;

        AudioData data = database.GetAudioData(bgmType);
        if (data == null) return;
        
        AudioClip nextClip = data.Clip;
        if (nextClip == null) return;

        BGM_Player.clip = nextClip;
        BGM_Player.Play();
    }

    public void SetMasterVolume(float amount)
    {
        if (audioMixer == null) return;
        audioMixer.SetFloat("Master", Mathf.Log10(amount) * 20.0f);
    }
    public void SetBGMVolume(float amount)
    {
        if (audioMixer == null) return;
        audioMixer.SetFloat("BGM", Mathf.Log10(amount) * 20.0f);
    }
    public void SetSFXVolume(float amount)
    {
        if (audioMixer == null) return;
        audioMixer.SetFloat("SFX", Mathf.Log10(amount) * 20.0f);
    }

    private void ChangeBGM(SceneType sceneType)
    {
        if (BGM_Player == null || database == null) return;

        // 씬 타입에 맞는 배경음 변경
        BGMType bgmType = ConvertToBGMType(sceneType);
        if(bgmType == BGMType.None) return;

        PlayBGM(bgmType);
    }

    private BGMType ConvertToBGMType(SceneType sceneType)
    {
        switch(sceneType)
        {
            case SceneType.Title:
                return BGMType.Title;
            case SceneType.Main:
                return BGMType.Main;
            default:
                return BGMType.None;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum VolumnType
{
    Master, BGM, SFX
}
public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Fields")]
    [SerializeField] private AudioSource BGM_Player;
    [SerializeField] private AudioSource SFX_Player;
    [SerializeField] private AudioDataBase audioDatabase;
    [SerializeField] private AudioMixer audioMixer;

    private Dictionary<VolumnType, string> Volumns = new Dictionary<VolumnType, string>()
    {
        {VolumnType.Master, "Master" },
        {VolumnType.BGM, "BGM" },
        {VolumnType.SFX, "SFX" }
    };

    protected override void Awake()
    {
        base.Awake();
        if(audioDatabase != null)
        {
            audioDatabase.Init();
        }
        if(audioMixer == null)
        {
            audioMixer = Resources.Load<AudioMixer>("AudioMixer");
        }
    }

    private void Start()
    {
        if (SceneLoadManager.Instance != null)
        {
            SceneLoadManager.Instance.OnSceneChanged += ChangeBGM;
            BGMType bgm = ConvertToBGMType(SceneLoadManager.Instance.CurrentScene);
            Play(bgm);
        }
    }
    private void OnDestroy()
    {
        if (SceneLoadManager.Instance != null)
            SceneLoadManager.Instance.OnSceneChanged -= ChangeBGM;
    }

    /// <summary>
    /// sfx타입을 받아 전달 해주는 메서드
    /// </summary>
    public AudioClip GetSFX(SFXType sfxType)
    {
        if (audioDatabase == null) return null;

        AudioData data = audioDatabase.GetAudioData(sfxType);
        if (data == null) return null;

        AudioClip nextClip = data.Clip;
        return nextClip;
    }

    // UI같은 전역 효과음을 사용하는 용도로 사용되는 메서드
    public void Play(SFXType sfxType)
    {
        if (SFX_Player == null) return;

        AudioClip nextClip = GetSFX(sfxType);
        if (nextClip == null) return;

        SFX_Player.PlayOneShot(nextClip);
    }

    /// <summary>
    /// bgm타입을 받아서 재생 해주는 메서드
    /// </summary>
    public void Play(BGMType bgmType)
    {
        if (BGM_Player == null || audioDatabase == null) return;

        AudioData data = audioDatabase.GetAudioData(bgmType);
        if (data == null) return;
        
        AudioClip nextClip = data.Clip;
        if (nextClip == null) return;

        BGM_Player.clip = nextClip;
        BGM_Player.Play();
    }
    
    // 타입에 맞는 볼륨을 조절하는 메서드
    public void SetVolume(VolumnType volumnType, float amount)
    {
        if (audioMixer == null) return;
        if (Volumns.TryGetValue(volumnType, out string name))
            audioMixer.SetFloat(name, Mathf.Log10(amount) * 20.0f);
    }

    // 타입에 맞는 볼륨의 값을 원본 값으로 변환해서 전달하는 메서드. 성공시 true, 실패시 false
    public bool TryGetVolumn(VolumnType volumnType, out float amount)
    {
        if (audioMixer == null)
        {
            amount = -1;
            return false;
        }
        if (Volumns.TryGetValue(volumnType, out string name))
        {
            if(audioMixer.GetFloat(name, out amount))
            {
                amount = Mathf.Pow(10, amount / 20f);
                return true;
            }
        }
        amount = -1;
        return false;
    }

    private void ChangeBGM(SceneType sceneType)
    {
        if (BGM_Player == null || audioDatabase == null) return;

        // 씬 타입에 맞는 배경음 변경
        BGMType bgmType = ConvertToBGMType(sceneType);
        if(bgmType == BGMType.None) return;

        Play(bgmType);
    }

    private BGMType ConvertToBGMType(SceneType sceneType)
    {
        switch(sceneType)
        {
            case SceneType.Title:
                return BGMType.Title;
            case SceneType.Main:
                return BGMType.Main;
            case SceneType.Clear:
                return BGMType.Clear;
            default:
                return BGMType.None;
        }
    }
}

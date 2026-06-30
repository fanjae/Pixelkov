using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDataBase", menuName = "Audio/Audio Data Base")]
public class AudioDataBase : ScriptableObject
{
    [SerializeField] private List<AudioData> AudioDataList;

    private Dictionary<BGMType, AudioData> BGMDatas = new Dictionary<BGMType, AudioData>();
    private Dictionary<SFXType, AudioData> SFXDatas = new Dictionary<SFXType, AudioData>();

    public void Init()
    {
        if (BGMDatas == null) BGMDatas = new Dictionary<BGMType, AudioData>();
        if (SFXDatas == null) SFXDatas = new Dictionary<SFXType, AudioData>();

        BGMDatas.Clear();
        SFXDatas.Clear();

        foreach (var data in AudioDataList)
        {
            if (data == null) continue;

            if(data.AudioType == AudioType.BGM)
            {
                if (!BGMDatas.TryAdd(data.BgmType, data)) Debug.LogWarning("중복된 타입의 오디오 클립 추가 오류");
            }
            else
            {
                if (!SFXDatas.TryAdd(data.SFXType, data)) Debug.LogWarning("중복된 타입의 오디오 클립 추가 오류");
            }
        }
    }

    public AudioData GetAudioData(BGMType type)
    {
        if(BGMDatas.ContainsKey(type))
            return BGMDatas[type];
        else
            return null;
    }
    public AudioData GetAudioData(SFXType type)
    {
        if(SFXDatas.ContainsKey(type))
            return SFXDatas[type];
        else
            return null;
    }
}

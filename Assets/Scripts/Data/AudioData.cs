using UnityEngine;

public enum AudioType
{
    BGM, SFX
}
public enum BGMType
{
    None, Title, Main, Clear, Shop, Boss
}
public enum SFXType
{
    None, 
    Equip, Use, PopUp, Buy, Sell, Craft,    // UI
    Walk, Shoot, Reload,    // Player
    EnemyWalk, EnemyAttack, // Enemy
}

[CreateAssetMenu(fileName = "AudioData", menuName = "Audio/Audio Data")]
public class AudioData : ScriptableObject
{
    [Header("Audio Clip")]
    [SerializeField] private AudioClip clip;

    [Header("Audio Clip Type")]
    [SerializeField] private AudioType audioType;
    [SerializeField] private BGMType bgmType;
    [SerializeField] private SFXType sfxType;

    public AudioClip Clip => clip;
    public AudioType AudioType => audioType;
    public BGMType BgmType => bgmType;
    public SFXType SFXType => sfxType;
}

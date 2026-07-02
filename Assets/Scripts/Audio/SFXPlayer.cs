using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        if (audioSource == null)
        {
            if(!TryGetComponent(out audioSource))
            {
                return;
            }
        }
        audioSource.outputAudioMixerGroup = AudioManager.Instance.GetMixerGroup();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
    }
    public void PlaySFX(SFXType sfxType)
    {
        if (audioSource == null || AudioManager.Instance == null) return;

        AudioClip clip = AudioManager.Instance.GetSFX(sfxType);
        if (clip == null) return;

        audioSource.PlayOneShot(clip);
    }
}

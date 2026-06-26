using UnityEngine;
using UnityEngine.UI;

public class VolumnPanel : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if(AudioManager.Instance != null)
        {
            if (masterSlider != null)
                masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
            if (bgmSlider != null)
                bgmSlider.onValueChanged.AddListener(AudioManager.Instance.SetBGMVolume);
            if (sfxSlider != null)
                sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
        }
    }
}

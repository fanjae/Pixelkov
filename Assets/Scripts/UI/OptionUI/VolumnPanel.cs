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
            // 슬라이더의 value를 초기화하고 onValueChanged에 이벤트 할당
            if (masterSlider != null)
            {
                if(AudioManager.Instance.TryGetVolumn(VolumnType.Master, out float volumnValue))
                    masterSlider.value = volumnValue;

                masterSlider.onValueChanged.AddListener((amount) => AudioManager.Instance.SetVolume(VolumnType.Master, amount));
            }
            if (bgmSlider != null)
            {
                if(AudioManager.Instance.TryGetVolumn(VolumnType.BGM, out float volumnValue))
                    bgmSlider.value = volumnValue;
                
                bgmSlider.onValueChanged.AddListener((amount) => AudioManager.Instance.SetVolume(VolumnType.BGM, amount));
            }
            if (sfxSlider != null)
            {
                if(AudioManager.Instance.TryGetVolumn(VolumnType.SFX, out float volumnValue))
                    sfxSlider.value = volumnValue;

                sfxSlider.onValueChanged.AddListener((amount) => AudioManager.Instance.SetVolume(VolumnType.SFX, amount));
            }
        }
    }
}

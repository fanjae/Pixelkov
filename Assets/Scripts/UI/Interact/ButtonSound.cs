using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void PlayButtonSound()
    {
        if (AudioManager.Instance == null) return;
        AudioManager.Instance.Play(SFXType.PopUp);
    }
}

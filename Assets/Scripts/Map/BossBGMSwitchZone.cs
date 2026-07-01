using UnityEngine;

public class BGMSwitchZone : MonoBehaviour
{
    [SerializeField] private BGMType enterBGM;
    [SerializeField] private BGMType exitBGM;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Play(enterBGM);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Play(exitBGM);
        }
    }
}
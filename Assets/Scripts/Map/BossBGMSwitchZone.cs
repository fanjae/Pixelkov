using Enemy1;
using UnityEngine;

public class BGMSwitchZone : MonoBehaviour
{
    [SerializeField] private BGMType enterBGM;
    [SerializeField] private BGMType exitBGM;

    [Header("보스")]
    [SerializeField] private EnemyBossController boss;

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

        // 보스 사망 시에는 BGM 바꾸지 않음
        if (boss.IsDead) return;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Play(exitBGM);
        }
    }
}
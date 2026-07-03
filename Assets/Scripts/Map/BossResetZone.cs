using Enemy1;
using UnityEngine;

public class BossResetZone : MonoBehaviour
{
    [SerializeField] private EnemyBossController boss;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (boss == null) return;
        if (boss.IsDead) return;

        boss.ResetBoss();
    }
}
using UnityEngine;

public class ShootingDisableZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerShooter shooter = collision.GetComponent<PlayerShooter>(); // 플레이어 슛팅 관련 컴포넌트를 찾아서 블록 처리
        if (shooter != null)
        {
            shooter.SetShootingBlocked(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerShooter shooter = collision.GetComponent<PlayerShooter>(); // 플레이어 슛팅 관련 컴포넌트를 찾아서 활성화 처리
        if (shooter != null)
        {
            shooter.SetShootingBlocked(false);
        }
    }
}
using UnityEngine;

public class MapTeleportZone : MonoBehaviour
{
    // 텔레포트 위치
    [SerializeField] private Transform targetPoint;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player 만 텔레포트
        if (!other.CompareTag("Player")) return;
        if (targetPoint == null) return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Rigidbody 2D 위치 이동
            rb.position = targetPoint.position;

            // 텔레포트 직후 속도 초기화
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            // Rigidbody 없으면 Transform 위치 변경
            other.transform.position = targetPoint.position;
        }
    }
}
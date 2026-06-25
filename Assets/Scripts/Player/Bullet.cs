using UnityEngine;
using static NewMonoBehaviourScript;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Header("투사체 설정")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifeTime = 10f;
    [SerializeField] private int damage = 1;

    [Header("시야 설정")]
    [SerializeField] private float visionDistance = 6f;

    private Rigidbody2D rb;
    private Transform player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // 안전장치로 일정 시간이 지나면 자동 삭제
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (player == null)
            return;

        // 플레이어와 투사체 사이 거리 확인
        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        // 시야 범위 밖으로 나가면 삭제
        if (distance > visionDistance)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;

        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }

        if (other.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    public void Launch(Vector2 direction)
    {
        // 대각선 방향에서도 같은 속도 유지
        direction = direction.normalized;

        // 전달받은 방향으로 투사체 이동
        rb.linearVelocity = direction * speed;

        // 이동 방향을 각도로 변환
        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 투사체가 이동 방향을 바라보도록 회전
        transform.rotation =
            Quaternion.Euler(0f, 0f, angle - 45f);
    }

  
}
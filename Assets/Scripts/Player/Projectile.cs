using UnityEngine;

// 이 스크립트가 붙은 오브젝트에는 Rigidbody2D가 반드시 필요함
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Header("투사체 설정")]

    // 투사체 속도
    [SerializeField] private float speed = 5f;

    // 투사체가 유지시간
    [SerializeField] private float lifeTime = 10f;

    // 투사체 데미지
    [SerializeField] private int damage = 1;

    private Rigidbody2D rb;

    private void Awake()
    {
        // 현재 투사체 오브젝트에 붙어 있는 Rigidbody2D를 가져옴
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 투사체가 영원히 남아 있지 않도록
        // lifeTime초가 지나면 자동으로 삭제
        Destroy(gameObject, lifeTime);
    }

    // 투사체가 발사 방향
    public void Launch(Vector2 direction)
    {
        // 대각선 방향에서도 속도 유지
        direction = direction.normalized;

        // 전달받은 방향으로 투사체를 이동시킴
        rb.linearVelocity = direction * speed;

        // 방향 벡터를 각도로 변환
        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 투사체 이미지가 오른쪽을 바라보는 기준일 때
        // 이동 방향을 향하도록 회전
        transform.rotation =
            Quaternion.Euler(0f, 0f, angle);
    }

    // 투사체의 Trigger Collider가 다른 Collider와 닿았을 때 실행
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌한 경우에는 투사체를 삭제하지 않음
        // 발사 직후 플레이어 자신의 Collider와 닿는 문제를 방지
        if (other.CompareTag("Player"))
            return;

        // 나중에 적 스크립트를 만들면 아래처럼 데미지를 전달할 수 있음
        /*
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        */

        // 벽, 적, 다른 장애물과 충돌하면 투사체 삭제
        Destroy(gameObject);
    }
}
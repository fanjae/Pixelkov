using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [Header("총알 설정")]
    [SerializeField] private float speed = 10f;

    [Header("삭제 설정")]
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private float maxDistance = 12f;
    //투사체 데미지
    [SerializeField] private int damage = 1;
    private Rigidbody2D rb;

    // 총알이 생성된 위치
    private Vector2 spawnPosition;

    // 총알이 생성된 뒤 흐른 시간
    private float lifeTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // 생성 위치 저장
        spawnPosition = transform.position;
    }

    private void Update()
    {
        // 생존 시간 증가
        lifeTimer += Time.deltaTime;

        // 생성 위치로부터 이동한 거리 계산
        float distance = Vector2.Distance(
            spawnPosition,
            transform.position
        );

        // 제한 시간 또는 제한 거리를 넘으면 삭제
        if (lifeTimer >= lifeTime || distance >= maxDistance)
        {
            Debug.Log("Bullet 삭제: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction)
    {
        if (rb == null)
        {
            return;
        }

        rb.linearVelocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어 자신과 충돌하면 무시
        if (other.CompareTag("Player"))
        {
            return;
        }

        // 충돌한 오브젝트나 부모에서 EnemyController 찾기
        EnemyController enemyController =
            other.GetComponentInParent<EnemyController>();

        // 적이라면 데미지 전달
        if (enemyController != null)
        {
            enemyController.TakeDamage(damage);
        }

        // 충돌 후 총알 삭제
        Destroy(gameObject);
    }
}
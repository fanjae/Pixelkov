using Enemy_Player;
using UnityEngine;

namespace Enemy1
{
    public class EnemyBullet : MonoBehaviour
    {
        [Header("Bullet 속도")]
        [SerializeField] private float moveSpeed = 10.0f;
        [Header("Bullet 생명주기")]
        [SerializeField] private float lifeTime = 3.0f;
        [Header("Damage")]
        [SerializeField] private int damage = 1;
        //벽 레이어
        [SerializeField] private LayerMask wallLayer;


        private Vector2 direction;

        public int Damage => damage;
        private bool isAttack = true;

        private void Start()
        {
            //일정시간 총알 삭제
            Destroy(gameObject, lifeTime);
        }

        //방향 설정
        public void SetDirection(Vector2 dir)
        {
            direction = dir;

            // 총알이 날아가는 방향으로 회전값 변경
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            // 스프라이트 기본 방향 보정
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 45f);
        }
        void Update()
        {
            // 지정된 방향으로 이동
            //transform.Translate(Vector2.right * moveSpeed * Time.deltaTime, Space.Self);
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);


        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isAttack) return;
            isAttack = false;

            // 벽 충돌 판정 추가
            if (((1 << collision.gameObject.layer) & wallLayer) != 0)
            {
                isAttack = true;
                Destroy(gameObject);
                return;
            }

            // 플레이어외의 테그는 무시
            if (!collision.CompareTag("Player"))
            {
                isAttack = true;
                return;
            }

            // 충돌한 오브젝트나 부모에서 EnemyController 찾기
            PlayerHealth enemyController =
                collision.GetComponentInParent<PlayerHealth>();
            
            //적이라면 데미지 전달
            if (enemyController != null)
            {
                enemyController.TakeDamage(damage);
            }
            isAttack = true;

            //// 충돌 후 총알 삭제
            Destroy(gameObject);
        }
    }
}

using Enemy_Player;
using UnityEngine;

namespace Enemy1
{
    public class EnemyBullet : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10.0f;
        [SerializeField] private float lifeTime = 3.0f;
        [SerializeField] private int damage = 1;

        
        private Vector2 direction;

        public int Damage => damage;
        private bool isAttack = false;

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
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        void Update()
        {
            // 지정된 방향으로 이동
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime, Space.Self);
            isAttack = true;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isAttack) return;
            // 플레이어 자신과 충돌하면 무시
            if (collision.CompareTag("Enemy"))
            {
                return;
            }

            // 충돌한 오브젝트나 부모에서 EnemyController 찾기
            PlayerHealth enemyController =
                collision.GetComponentInParent<PlayerHealth>();

            // 적이라면 데미지 전달
            if (enemyController != null)
            {
                enemyController.TakeDamage(damage);
            }
            isAttack = false;

            // 충돌 후 총알 삭제
            Destroy(gameObject);
        }

        //프리펩 Flip 설정
        //public void SetFlip(int x, int y)
        //{
        //    spriteRenderer.flipX = x == 1 ? true : false;
        //    spriteRenderer.flipY = y == 1 ? true : false;
        //}
    }
}

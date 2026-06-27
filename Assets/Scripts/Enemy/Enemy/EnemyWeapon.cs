using Enemy_Player;
using UnityEngine;


namespace Enemy1
{
    public class EnemyWeapon : MonoBehaviour
    {
        [SerializeField] private int damage = 1;

        private bool isAttack = false;
        public int Damage => damage;

        private BoxCollider2D collider;

        private void Awake()
        {
            collider = GetComponent<BoxCollider2D>();
        }
        //공격 시작
        public void StartAttack()
        {
            isAttack = true;
            collider.enabled = true;
        }

        //공격 종료
        public void EndAttack()
        {
            isAttack = false;
            collider.enabled = false;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isAttack) return;

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
            //공격 종료
            EndAttack();
        }
    }
}

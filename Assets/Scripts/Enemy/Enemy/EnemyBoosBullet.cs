using UnityEngine;

namespace Enemy1
{
    public class EnemyBoosBullet : MonoBehaviour
    {
        [Header("Bullet 속도")]
        [SerializeField] private float moveSpeed = 10.0f;
        [Header("Bullet 생명주기")]
        [SerializeField] private float lifeTime = 3.0f;
        [Header("Damage")]
        [SerializeField] private int damage = 1;
        [Header("Sprite 회전 보정값")]
        [SerializeField] private SFXPlayer sfxPlayer;
        //벽 레이어
        [SerializeField] private LayerMask wallLayer;



        public int Damage => damage;
        private bool isAttack = true;

        void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        void Update()
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
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
                DamageHandler.EnemyAttack(transform.position, damage);
                enemyController.TakeDamage(damage);


                sfxPlayer.PlaySFX(SFXType.EnemyImpact);
            }
            isAttack = true;

            //// 충돌 후 총알 삭제
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

namespace Enemy_Player
{
    public class Enemy_PlayerBullet : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10.0f;
        [SerializeField] private float lifeTime = 3.0f;
        [SerializeField] private int damage = 1;


        private Vector2 direction;

        public int Damage => damage;
        void Start()
        {
            //bullet 삭제
            Destroy(gameObject, lifeTime);
        }
        void Update()
        {
            //bullet 이동
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }

        //bullet 방향
        public void SetDirection(Vector2 dir)
        {
            direction = dir.normalized;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log("TESt");
            //////if (!isAttack) return;
            //// 플레이어 자신과 충돌하면 무시
            //if (collision.CompareTag("Enemy"))
            //{
            //    return;
            //}
            //Debug.Log("TESt2");

            ////// 충돌한 오브젝트나 부모에서 EnemyController 찾기
            //IEnmeyController enemyController =
            //    collision.GetComponentInParent<IEnmeyController>();
            ////Enemy_PlayerController enemyController =
            ////    collision.GetComponentInParent<Enemy_PlayerController>();

            //// 적이라면 데미지 전달
            //if (enemyController != null)
            //{
            //    enemyController.TakeDamage(damage);
            //    Debug.Log("TESt3");
            //}
            //Debug.Log("TESt5");
            //////isAttack = false;

            ////// 충돌 후 총알 삭제
            //Destroy(gameObject);
        }
    }
}

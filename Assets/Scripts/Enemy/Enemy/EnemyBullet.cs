using UnityEngine;

namespace Enemy1
{
    public class EnemyBullet : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10.0f;
        [SerializeField] private float lifeTime = 3.0f;
        [SerializeField] private int damage = 1;

        SpriteRenderer spriteRenderer;
        private Vector2 direction;

        public int Damage => damage;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();        
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
        }

        //프리펩 Flip 설정
        public void SetFlip(int x, int y)
        {
            spriteRenderer.flipX = x == 1 ? true : false;
            spriteRenderer.flipY = y == 1 ? true : false;
        }
    }
}

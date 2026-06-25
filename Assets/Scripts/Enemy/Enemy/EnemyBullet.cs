using UnityEngine;

namespace Enemy1
{
    public class EnemyBullet : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10.0f;
        [SerializeField] private float lifeTime = 3.0f;
        [SerializeField] private int damage = 1;

        private SpriteRenderer spriteRenderer;
        private Vector2 direction;

        public int Damage => damage;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
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
        //프리펩 Flip 설정
        public void SetFlip(int x, int y)
        {
            spriteRenderer.flipX = x == 1 ? true : false;
            spriteRenderer.flipY = y == 1 ? true : false;
        }
    }
}

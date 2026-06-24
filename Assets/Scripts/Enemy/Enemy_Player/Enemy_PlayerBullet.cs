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
    }
}

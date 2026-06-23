using UnityEngine;

namespace Enemy1
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5.0f;
        [SerializeField] private EnemyAnimationController animationController;


        private Rigidbody2D rb;
        
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animationController = GetComponentInChildren<EnemyAnimationController>();
        }

        // Update is called once per frame
        void Update()
        {
            //애니메이션 상태 업데이트
            UpdateAnimation();
        }
        private void FixedUpdate()
        {
            //이동
            Move();
        }

        private void Move()
        {
            //X, Y로 이동
            float moveX = InputManager.Movement.x;
            float moveY = InputManager.Movement.y;

            //이동
            rb.linearVelocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed);
        }

        //애니메이션 상태 업데이트
        private void UpdateAnimation()
        {
            animationController.UpdateState(InputManager.Movement.x, InputManager.Movement.y);
        }
        
    }
}

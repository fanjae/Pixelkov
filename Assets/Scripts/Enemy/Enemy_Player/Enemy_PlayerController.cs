using UnityEngine;
namespace Enemy_Player
{
    public class Enemy_PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5.0f;
        [SerializeField] private Enemy_PlayerAnimationController animationController;


        private Rigidbody2D rb;
        private SpriteRenderer spriteRenderer;
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animationController = GetComponentInChildren<Enemy_PlayerAnimationController>();

        }

        
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


            rb.linearVelocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed);
        }
        //애니메이션 상태 업데이트
        private void UpdateAnimation()
        {
            animationController.UpdateState(InputManager.Movement.x, InputManager.Movement.y);
        }
    }
}

using UnityEngine;

namespace Enemy1
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5.0f;
        [SerializeField] private EnemyAnimationController animationController;
        [SerializeField] private EnemyShooterController shooterController;

        //플레이어와 적 공격 거리
        [SerializeField] private float fireDistance = 4.0f;

        //플레이어
        private Transform target;

        private Rigidbody2D rb;
        
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animationController = GetComponentInChildren<EnemyAnimationController>();
            shooterController = GetComponentInChildren<EnemyShooterController>();

            //플레이어 컴포넌틑
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        
        void Update()
        {
            //애니메이션 상태 업데이트
            //UpdateAnimation();
            //플레이어 포지션 전달
            UpdatePlayerShoter();

            //플레이어와 적 공격 거리 
            if (Vector2.Distance(transform.position, target.position) <= fireDistance)
            {

                //발사
                shooterController.Fire();
            }
        }
        private void FixedUpdate()
        {
            //이동
            //추적 시스템 전 주석
            //Move();
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
        //공격 방향
        private void UpdatePlayerShoter()
        {
            shooterController.UpdateShooterState(target.position);
        }

    }
}

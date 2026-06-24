using Enemy;
using Enemy_Player;
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

        //HP UI
        [SerializeField] private EnemyUI hpUI;
        //Max HP
        [SerializeField] private int maxHealth = 3;
        //초기 HP
        private int currentHealth;
        

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animationController = GetComponentInChildren<EnemyAnimationController>();
            shooterController = GetComponentInChildren<EnemyShooterController>();

            //플레이어 컴포넌트
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        void Start()
        {
            //HP셋팅
            currentHealth = maxHealth;
            if (hpUI != null)
            {
                hpUI.Initialize(maxHealth);
            }
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

        //Damage
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (hpUI != null)
            {
                hpUI.SetHP(currentHealth);
            }
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        //사망
        private void Die()
        {
            //Eenmy 삭제
            Destroy(gameObject);
        }
        //플레이어 공격 트리거
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //플레이어 공격 컴포넌트
            if (collision.TryGetComponent<Enemy_PlayerBullet>(out Enemy_PlayerBullet bullet))
            {
                //Damage
                TakeDamage(bullet.Damage);
                //공격 삭제
                Destroy(bullet.gameObject);
            }
        }

    }
}

using Enemy1;
using UnityEngine;
namespace Enemy_Player
{
    public class Enemy_PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5.0f;
        [SerializeField] private Enemy_PlayerAnimationController animationController;
        [SerializeField] private Enemy_PlayerShooterController shooterController;

        //HP UI
        [SerializeField] private Enemy_PlayerUI hpUI;
        //Max HP
        [SerializeField] private int maxHealth = 3;
        //초기 HP
        private int currentHealth;

        private Rigidbody2D rb;
        

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animationController = GetComponentInChildren<Enemy_PlayerAnimationController>();
            shooterController = GetComponentInChildren<Enemy_PlayerShooterController>();

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
            UpdateAnimation();
            //Shooter 방향 업데이트
            UpdatePlayerShoter();

            if (InputManager.IsFire)
            {
                shooterController.Fire();
            }
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
        //Shooter 방향 업데이트
        private void UpdatePlayerShoter()
        {
            shooterController.UpdateShooterState(InputManager.Movement.x, InputManager.Movement.y);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<EnemyBullet>(out EnemyBullet bullet))
            {
                Debug.Log("bullet attack");
                Destroy(bullet.gameObject);
            }
            if (collision.TryGetComponent<Weapon>(out Weapon weapon))
            {
                Debug.Log("weapon attack");
                //TakeDamage(weapon.Damage);
            }
        }

    }
}

using Enemy;
using Enemy_Player;
using UnityEngine;
namespace Enemy1
{
    public class EnemyWeaponController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2.0f;
        [SerializeField] private EnemyAnimationController animationController;
        

        //플레이어와 적 공격 거리
        [SerializeField] private float attackDistance = 0.5f;
        //플레이어 거리 기준 이동 거리
        [SerializeField] private float targeteDistance = 6.0f;

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


        private void FixedUpdate()
        {
            //플레이어 - 적 거리
            float distance = Vector2.Distance(transform.position, target.position);
            //이동 
            if (distance < targeteDistance)
            {
                //공격
                if (distance < attackDistance)
                {
                    //애니메이션 타입 : 공격
                    //현재 공격 애니메이션 문제로 주석
                    UpdateAnimation(EnemyActionType.Attack);                    
                    return;
                }
                //애니메이션 타입
                UpdateAnimation(EnemyActionType.Move);
                Move();
                return;
            }
            //애니메이션 타입
            UpdateAnimation(EnemyActionType.Idle);
        }

        private void Move()
        {
            
            //플레이어와 거리
            Vector2 direction = (target.position - transform.position).normalized;
            //플레이어 방향으로 이동
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }

        //애니메이션 상태 업데이트
        //para : action type
        private void UpdateAnimation(EnemyActionType actionType)
        {
            animationController.UpdateState(actionType);
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
        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(transform.position, targeteDistance);
        //}

    }
}

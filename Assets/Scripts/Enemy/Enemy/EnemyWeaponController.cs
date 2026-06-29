using Enemy;
using Enemy_Player;
using UnityEngine;
using System.Collections;
namespace Enemy1
{
    public class EnemyWeaponController : MonoBehaviour, IEnmeyController
    {
        [SerializeField] private float moveSpeed = 2.0f;
        [SerializeField] private EnemyAnimationController animationController;
        [SerializeField] private EnemyWeapon weapon;
        //골드 컨드롤
        [SerializeField] private PlayerGoldController goldController;

        //플레이어와 적 공격 거리
        [SerializeField] private float attackDistance = 0.5f;
        //플레이어 거리 기준 이동 거리
        [SerializeField] private float targeteDistance = 6.0f;

        //코인
        [SerializeField]private GameObject coin;
        //골드
        [SerializeField] private int gold;

        //플레이어
        private Transform target;

        //HP UI
        [SerializeField] private EnemyUI hpUI;
        //Max HP
        [SerializeField] private int maxHealth = 3;
        //초기 HP
        private int currentHealth;

        private Collider2D collider;
        private Rigidbody2D rb;
        

        private bool isDead = false;
        private bool isAttack = false;

        private void Awake()
        {
            
            collider = GetComponent<Collider2D>();
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
            if (isDead) return;
            if (isAttack) return;
            float distance = Vector2.Distance(transform.position, target.position);
            if (distance < targeteDistance)
            {
                if (distance < attackDistance)
                {
                    //공격 코루틴
                    StartCoroutine(AttackRoutine());
                    return;
                }
                UpdateAnimation(EnemyActionType.Move);
                Move();
                return;
            }
            UpdateAnimation(EnemyActionType.Idle);
        }
        //공격 코루틴
        IEnumerator AttackRoutine()
        {
            isAttack = true;
            UpdateAnimation(EnemyActionType.Attack);
            weapon.StartAttack();
            yield return new WaitForSeconds(1.0f);
            isAttack = false;

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
            if (isDead) return;
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
            isDead = true;
            //코인 Instantiate
            Instantiate(coin, transform.position, Quaternion.identity);
            //골드 추가
            goldController.AddGold(gold);
            //Dead 애니메이션
            UpdateAnimation(EnemyActionType.Dead);
            //hp 삭제
            Destroy(transform.Find("HP").gameObject);

            //collider, Rigidbody 비활성화
            collider.enabled = false;
            rb.simulated = false;
            Destroy(gameObject, 3.0f);

        }

        //충돌시 밀어내기
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 충돌한 대상이 Enemy 
            if (collision.gameObject.layer == LayerMask.NameToLayer("enemy"))
            {
                Rigidbody2D otherRb = collision.GetComponent<Rigidbody2D>();
                if (otherRb != null)
                {
                    // 밀어낼 방향과 거리 계산
                    Vector2 pushDir = (Vector2)(transform.position - collision.transform.position).normalized;
                    // 위치 강제 이동
                    transform.position += (Vector3)(pushDir * 0.1f);
                }
            } 
        }
    }
}

using Enemy;
using Enemy_Player;
using UnityEngine;
using System.Collections;


namespace Enemy1
{
    public class EnemyController : MonoBehaviour, IEnmeyController
    {
        
        [SerializeField] private EnemyAnimationController animationController;
        [SerializeField] private EnemyShooterController shooterController;
        //골드 컨드롤
        [SerializeField] private PlayerGoldController goldController;
        //HP UI
        [SerializeField] private EnemyUI hpUI;
        //코인
        [SerializeField] private GameObject coin;

        [Header("이동 속도")]
        [SerializeField] private float moveSpeed = 2.0f;
        //플레이어와 적 공격 거리
        [Header("Player 이동 시작 거리 ")]
        [SerializeField] private float fireDistance = 3.0f;
        //플레이어 거리 기준 이동 거리
        [Header("원거리 공격 시작 거리")]
        [SerializeField] private float targeteDistance = 6.0f;
        //골드
        [Header("Gold")]
        [SerializeField] private int gold;
        //Max HP
        [Header("최대 HP")]
        [SerializeField] private int maxHealth = 3;

        //플레이어
        private Transform target;

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


        private void FixedUpdate()
        {
            if (isDead) return;
            if (isAttack) return;
            //플레이어 사망후 대기 상태
            if (target.GetComponentInParent<PlayerHealth>().CurrentHealth == 0
                || target == null)
            {
                //애니메이션 타입
                UpdateAnimation(EnemyActionType.Idle);
                return;
            }
            //플레이어 - 적 거리
            float distance = Vector2.Distance(transform.position, target.position);

            if (distance < targeteDistance)
            {
                if (distance < fireDistance)
                {
                    //공격 코루틴
                    StartCoroutine(AttackRoutine());
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
        //공격 코루틴
        IEnumerator AttackRoutine()
        {
            isAttack = true;
            //애니메이션 타입 : 공격
            //현재 공격 애니메이션 문제로 주석
            UpdateAnimation(EnemyActionType.Attack);
            ////공격 방향
            //UpdatePlayerShoter();
            ////공격

            yield return new WaitForSeconds(1.0f);
            shooterController.Fire();
            //공격후 딜레이
            //yield return new WaitForSeconds(1.0f);
            isAttack = false;

        }

        private void Move()
        {
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
            //Eenmy 삭제
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

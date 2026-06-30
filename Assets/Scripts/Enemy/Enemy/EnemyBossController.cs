using Enemy;
using UnityEngine;
using System.Collections;
namespace Enemy1
{
    public class EnemyBossController : MonoBehaviour, IEnmeyController
    {
        
        [SerializeField] private EnemyAnimationController animationController;
        [SerializeField] private EnemyShooterController shooterController;
        [SerializeField] private EnemyWeapon weapon;
        //골드 컨드롤
        [SerializeField] private PlayerGoldController goldController;
        //HP UI
        [SerializeField] private EnemyUI hpUI;
        //코인
        [SerializeField] private GameObject coin;
        //회복 아이콘 
        [SerializeField] private GameObject hpAdd;


        [Header("이동 속도")]
        [SerializeField] private float moveSpeed = 2.0f;
        //플레이어 거리 기준 이동 거리
        [Header("Player 이동 시작 거리 ")]
        [SerializeField] private float targeteDistance = 6.0f;
        //플레이어와 적 공격 거리
        [Header("원거리 공격 시작 거리")]
        [SerializeField] private float fireDistance = 3.0f;
        //근접무기 공격 거리
        [Header("근접 공격 시작 거리")]
        [SerializeField] private float attackDistance = 0.5f;
        //골드
        [Header("Gold")]
        [SerializeField] private int gold;
        //Max HP
        [Header("최대 HP")]
        [SerializeField] private int maxHealth = 3;
        //HP 회복시간
        [Header("HP 회복시간")]
        [SerializeField] private float recoveryHPDelay = 5.0f;
        //Dash 재사용 시간
        [Header("Dash 재사용 시간")]
        [SerializeField] private float dashDelay = 5.0f;
        //Dash 스피드
        [Header("Dash 스피드")]
        [SerializeField] public float dashSpeed = 15.0f;


        //플레이어
        private Transform target;
        //초기 HP
        private int currentHealth;
        //대쉬 지속 시간
        private float dashDuration = 0.2f;

        //Dash 타이머
        private float dashTimer = 0.0f;
        //HP 타이머
        private float recoveryHPTimer = 0.0f;

        private Collider2D collider;
        private Rigidbody2D rb;

        private bool isDead = false;
        private bool isAttack = false;

        //원래 자리
        private Vector2 originalPosiotion;

        private void Awake()
        {
            originalPosiotion = transform.position;

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

        private void Update()
        {
            
            dashTimer += Time.deltaTime;
            //데미지 있을시 HP 회복 타이머 작동
            if (currentHealth < maxHealth)
            {
                
                recoveryHPTimer += Time.deltaTime;
                Debug.Log(recoveryHPTimer);
            }
            //HP 회복
            if (recoveryHPDelay < recoveryHPTimer
                && currentHealth < maxHealth
                )
            {
                currentHealth += 1;
                hpUI.SetHP(currentHealth);
                recoveryHPTimer = 0.0f;
                //HP 회복 아이콘 
                StartCoroutine(RecoveryHPRoutine());
            }
        }

        IEnumerator RecoveryHPRoutine()
        {

            hpAdd.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            hpAdd.SetActive(false);

        }

        private void FixedUpdate()
        {
            if (isDead) return;
            if (isAttack) return;
            //플레이어 - 적 거리
            float distance = Vector2.Distance(transform.position, target.position);
            
            if (distance < targeteDistance)
            {
                if (3 < currentHealth)
                {
                    if (distance < fireDistance)
                    {
                        //원거리 공격 루틴
                        StartCoroutine(AttackRoutine());
                        return;
                    }
                }
                else
                {
                    if (fireDistance < distance && dashDelay <= dashTimer)
                    {
                        //데쉬 루틴
                        StartCoroutine(DashRoutine());
                        dashTimer = 0.0f;
                        return;
                    }
                    if (distance < attackDistance)
                    {
                        
                        //근접무기 공격 코루틴
                        StartCoroutine(AttackWeaponRoutine());
                        return;
                    }
                }
                //애니메이션 타입
                UpdateAnimation(EnemyActionType.Move);
                Move();
                return;
            }
            //플레이어와 거리가 멀어지면 원래 자리로 이동
            if (transform.position.x !=originalPosiotion.x
                && transform.position.y != originalPosiotion.y)
            {
                UpdateAnimation(EnemyActionType.Move);
                OriMove();
                return;
            }
            //애니메이션 타입
            UpdateAnimation(EnemyActionType.Idle);

        }

        IEnumerator DashRoutine()
        {
            isAttack = true;
            Vector2 dashDirection = (target.position - transform.position).normalized;

            Vector2 dashVelocity = dashDirection * dashSpeed;

            // 3. 기존 속도를 덮어씌워 일정하게 대쉬
            rb.linearVelocity = dashVelocity;

            yield return new WaitForSeconds(dashDuration);

            // 대쉬 종료 후 속도 초기화
            rb.linearVelocity = Vector2.zero;

            isAttack = false;

        }
        //원거리 공격
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

        IEnumerator AttackWeaponRoutine()
        {
            isAttack = true;
            UpdateAnimation(EnemyActionType.WeaponAttack);
            weapon.StartAttack();
            yield return new WaitForSeconds(1.0f);
            isAttack = false;

        }
        //근접무기 공격
        

        
        private void OriMove()
        {
            transform.position = Vector2.MoveTowards(transform.position, originalPosiotion, moveSpeed * Time.deltaTime);
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
            //골드 추가
            goldController.AddGold(gold);
            UpdateAnimation(EnemyActionType.Dead);
            //Eenmy 삭제
            Destroy(transform.Find("HP").gameObject);
            Instantiate(coin, transform.position, Quaternion.identity);

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


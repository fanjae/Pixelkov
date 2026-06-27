using Enemy;
using Enemy_Player;
using UnityEngine;
using System.Collections;


namespace Enemy1
{
    public class EnemyController : MonoBehaviour, IEnmeyController
    {
        [SerializeField] private float moveSpeed = 2.0f;
        [SerializeField] private EnemyAnimationController animationController;
        [SerializeField] private EnemyShooterController shooterController;

        //플레이어와 적 공격 거리
        [SerializeField] private float fireDistance = 3.0f;
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

        private bool isDead = false;
        private bool isAttack = false;

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


        private void FixedUpdate()
        {
            if (isDead) return;
            if (isAttack) return;
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
            UpdateAnimation(EnemyActionType.Dead);
            //Eenmy 삭제
            Destroy(transform.Find("HP").gameObject);
        }
    }
}

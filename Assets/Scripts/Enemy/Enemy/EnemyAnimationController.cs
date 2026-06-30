using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace Enemy1
{
    public class EnemyAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        //초기 애니메이션 타입
        private EnemyActionType currentState = EnemyActionType.Idle;

        private static readonly int MoveHash = Animator.StringToHash("Move");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int WeaponAttackHash = Animator.StringToHash("WeaponAttack");
        private static readonly int DeadHash = Animator.StringToHash("isDeath");

        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }
        //애니메이션 모션 업데이트
        //Para : 애니메이션 액션 타입
        public void UpdateState(EnemyActionType newState)
        {
            if (currentState == newState) return;
            currentState = newState;
            if (newState == EnemyActionType.Idle)
            {
                animator.SetBool(MoveHash, false);
                animator.SetBool(AttackHash, false);
                animator.SetBool(WeaponAttackHash, false);
            }
            else if (newState == EnemyActionType.Move)
            {
                animator.SetBool(MoveHash, true);
                animator.SetBool(AttackHash, false);
                animator.SetBool(WeaponAttackHash, false);
            }
            else if (newState == EnemyActionType.Attack)
            {
                animator.SetBool(MoveHash, false);
                animator.SetBool(AttackHash, true);
                animator.SetBool(WeaponAttackHash, false);
            }
            else if (newState == EnemyActionType.WeaponAttack)
            {
                animator.SetBool(MoveHash, false);
                animator.SetBool(AttackHash, false);
                animator.SetBool(WeaponAttackHash, true);
            }
            else if (newState == EnemyActionType.Dead)
            {
                animator.SetBool(MoveHash, false);
                animator.SetBool(AttackHash, false);
                animator.SetBool(WeaponAttackHash, false);
                animator.SetBool(DeadHash, true);
                animator.SetBool(DeadHash, true);
            }
        }
    }
}

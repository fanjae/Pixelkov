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
            }
            else if (newState == EnemyActionType.Move)
            {
                animator.SetBool(MoveHash, true);
                animator.SetBool(AttackHash, false);
            }
            else if (newState == EnemyActionType.Attack)
            {
                animator.SetBool(MoveHash, false);
                animator.SetBool(AttackHash, true);
            }
        }
    }
}

using UnityEngine;

namespace Enemy1
{
    public class EnemyAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        //초기 애니메이션 타입
        private EnemyActionType currentState = EnemyActionType.Idle;
        private static readonly int StateHash = Animator.StringToHash("Status");
        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }
        //애니메이션 모션 업데이트
        //Para : 애니메이션 액션 타입
        public void UpdateState(EnemyActionType actionType)
        {
            if (currentState == actionType) return;
            currentState = actionType;
            animator.SetInteger(StateHash, (int)actionType);
        }
    }
}

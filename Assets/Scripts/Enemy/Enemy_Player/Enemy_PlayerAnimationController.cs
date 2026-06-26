using UnityEngine;
namespace Enemy_Player
{
    public class Enemy_PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        //애니메이션 상태 
        //0 -> Idle
        //0 -> Move
        private static readonly int status = Animator.StringToHash("Status");
        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        //애니메이션 모션 업데이트
        public void UpdateState(float moveX, float moveY)
        {
            if (Mathf.Approximately(moveX, 0) && Mathf.Approximately(moveY, 0))
            {
                //0 -> Idle
                animator.SetInteger(status, 0);
            }
            else
            {
                //0 -> Move
                animator.SetInteger(status, 1);
            }


        }
    }
}

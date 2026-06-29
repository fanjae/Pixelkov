using UnityEngine;

public class Temp_EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator refAnimator;

    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Damaged = Animator.StringToHash("Damaged");
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Debuff = Animator.StringToHash("Debuff");
    private static readonly int Other = Animator.StringToHash("Other");
    private static readonly int IsDeath = Animator.StringToHash("isDeath");

    private void Awake()
    {
        refAnimator = GetComponentInParent<Animator>();

        if (refAnimator == null)
        {
            refAnimator = GetComponentInChildren<Animator>();
        }

        if (refAnimator == null)
        {
            Debug.LogWarning("Animator not found");
        }
    }

    public void SetMove(bool isMove)
    {
        if (refAnimator == null) return;

        refAnimator.SetBool(Move, isMove);
    }

    public void PlayAttack()
    {
        if (refAnimator == null) return;

        refAnimator.SetBool(Move, false);
        refAnimator.SetTrigger(Attack);
    }

    public void PlayDamaged()
    {
        if (refAnimator == null) return;

        refAnimator.SetTrigger(Damaged);
    }

    public void PlayDeath()
    {
        if (refAnimator == null) return;

        refAnimator.SetBool(Move, false);
        refAnimator.SetBool(IsDeath, true);
        refAnimator.SetTrigger(Death);
    }

    public void SetDebuff(bool isDebuff)
    {
        if (refAnimator == null) return;

        refAnimator.SetBool(Debuff, isDebuff);
    }

    public void PlayOther()
    {
        if (refAnimator == null) return;

        refAnimator.SetTrigger(Other);
    }
}
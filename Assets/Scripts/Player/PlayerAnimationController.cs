using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer characterRenderer;

    public void SetMoveDirection(Vector2 moveInput)
    {
        // 오른쪽 이동
        if (moveInput.x > 0.01f)
        {
            characterRenderer.flipX = false;
        }
        // 왼쪽 이동
        else if (moveInput.x < -0.01f)
        {
            characterRenderer.flipX = true;
        }
    }
}
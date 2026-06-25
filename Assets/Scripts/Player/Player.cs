using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("이동 설정")]//이동속도
    [SerializeField] private float moveSpeed = 5f;

    [Header("회피 설정")]//회피 속도,지속*무적 시간, 횟수, 횟수충전시간
    [SerializeField] private float dodgeSpeed = 15f;
    [SerializeField] private float dodgeDuration = 0.2f;
    [SerializeField] private float invincibleTime = 0.2f;
    [SerializeField] private int maxDodgeCount = 2;
    [SerializeField] private float dodgeRecoverTime = 3f;

    private Rigidbody2D rb; 
    [Header("애니메이터")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform characterVisual;
    //현재 이동방향,마지막 이동방향, 회피방향, 마우스 방향
    private Vector2 moveInput;
    private Vector2 lastDirection = Vector2.down;
    private Vector2 dodgeDirection;
    private Camera mainCamera;
    private Vector2 aimDirection = Vector2.down;

    private bool isDodging;
    private bool isInvincible;
    private int currentDodgeCount;

    private PlayerHealth playerHealth;
    //타 스크립트에서 무적이랑 횟수 확인시 사용
    public bool IsInvincible => isInvincible;
    public int CurrentDodgeCount => currentDodgeCount;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        mainCamera = Camera.main;

        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        currentDodgeCount = maxDodgeCount;
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        ReadAimDirection();
        // 회피 중에는 일반 이동 입력을 받지 않음
        // 회피 중에 마우스 방향 바라보는 애니메이션 추가
        if (isDodging)
        { animator.SetFloat("MoveX", aimDirection.x);
        animator.SetFloat("MoveY", aimDirection.y);
        return;
        }
        ReadMovementInput();
        UpdateCharacterDirection();
        UpdateAnimation();

        // Space를 누르면 회피
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TryDodge();
        }
    }
    private void UpdateCharacterDirection()
    {
        if (characterVisual == null)
            return;

        // 오른쪽 이동
        if (moveInput.x > 0.01f)
        {
            characterVisual.localScale =
                new Vector3(-0.1f, 0.1f, 0.1f);
        }
        // 왼쪽 이동
        else if (moveInput.x < -0.01f)
        {
            characterVisual.localScale =
                new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
    private void FixedUpdate()
    {
        if (isDodging)
        {
            rb.linearVelocity = dodgeDirection * dodgeSpeed;
        }
        else
        {
            rb.linearVelocity = moveInput * moveSpeed;
        }
    }

    private void ReadMovementInput()
    {
        moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            moveInput.y += 1f;

        if (Keyboard.current.sKey.isPressed)
            moveInput.y -= 1f;

        if (Keyboard.current.aKey.isPressed)
            moveInput.x -= 1f;

        if (Keyboard.current.dKey.isPressed)
            moveInput.x += 1f;

        moveInput = moveInput.normalized;

        if (moveInput != Vector2.zero)
        {
            lastDirection = moveInput;
        }
    }

    private void UpdateAnimation()
    {
        bool isMoving = moveInput != Vector2.zero;

       
            animator.SetFloat("MoveX", lastDirection.x);
            animator.SetFloat("MoveY", lastDirection.y);
        

        animator.SetBool("IsMoving", isMoving);
    }

    private void TryDodge()
    {
        if (isDodging)
            return;

        if (currentDodgeCount <= 0)
            return;

        // 움직이는 중이면 현재 이동 방향으로 회피
        // 멈춘 상태면 마지막으로 바라본 방향으로 회피
        dodgeDirection =
            moveInput != Vector2.zero
                ? moveInput
                : aimDirection;

        currentDodgeCount--;

        StartCoroutine(DodgeCoroutine());
        StartCoroutine(RecoverDodgeCharge());
    }

    private IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        isInvincible = true;

       
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsDodging", true);

        float timer = 0f;
        //회피 지속 시간
        while (timer < dodgeDuration)
        {
            timer += Time.deltaTime;

            if (timer >= invincibleTime)
            {
                isInvincible = false;
            }

            yield return null;
        }

        rb.linearVelocity = Vector2.zero;

        isInvincible = false;
        isDodging = false;

        animator.SetBool("IsDodging", false);
    }

    //
    private void ReadAimDirection()
    {
        if (mainCamera == null || Mouse.current == null)
            return;

        Vector3 mouseWorldPosition =
            mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        mouseWorldPosition.z = 0f;

        Vector2 direction =
            (mouseWorldPosition - transform.position).normalized;

        if (direction != Vector2.zero)
        {
            aimDirection = direction;
        }
    }

    // 일정 시간이 지나면 회피 횟수 1개 회복
    private IEnumerator RecoverDodgeCharge()
    {
        yield return new WaitForSeconds(dodgeRecoverTime);

        currentDodgeCount++;

        if (currentDodgeCount > maxDodgeCount)
        {
            currentDodgeCount = maxDodgeCount;
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("회피 설정")]
    [SerializeField] private float dodgeSpeed = 15f;
    [SerializeField] private float dodgeDuration = 0.2f;
    [SerializeField] private float invincibleTime = 0.2f;
    [SerializeField] private int maxDodgeCount = 3;
    [SerializeField] private float dodgeRecoverTime = 3f;

    [Header("애니메이터")]
    [SerializeField] private Animator animator;

    [Header("캐릭터 방향")]
    [SerializeField] private Transform horseRoot;

    // 0626 (테스트용)
    [Header("인벤토리 테스트")]
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private int inventorySlotCount = 12;

    [SerializeField] private int defaultDamage = 1;

    public PlayerInventoryController InventoryController { get; private set; }
    public PlayerWeaponController WeaponController { get; private set; }
    private Coroutine dodgeRecoverCoroutine;

    public Inventory Inventory { get; private set; }
    public Equipment Equipment { get; private set; }
    private PlayerHealth playerHealth;

    // 0626 (테스트용)

    // 원본 말 이미지가 기본적으로 오른쪽을 보고 있으면 체크
    // 기본적으로 왼쪽을 보고 있으면 체크 해제
    [SerializeField] private bool horseFacesRightByDefault;

    private Rigidbody2D rb;
    private Camera mainCamera;

    // 현재 이동 방향
    private Vector2 moveInput;

    // 마지막으로 이동했던 방향
    private Vector2 lastDirection = Vector2.down;

    // 회피 방향
    private Vector2 dodgeDirection;

    // 마우스 조준 방향
    private Vector2 aimDirection = Vector2.down;

    private bool isDodging;
    private bool isInvincible;
    private int currentDodgeCount;

    // HorseRoot가 처음 가지고 있던 크기
    private Vector3 horseOriginalScale;

    public bool IsInvincible => isInvincible;
    public int CurrentDodgeCount => currentDodgeCount;

    // 장비 장착 부분에 대한 이벤트 해제
    private void OnDestroy()
    {
        if (Equipment != null)
        {
            Equipment.OnEquipmentChanged -= UpdateDefenseFromEquipment;
        }
    }
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

        // HorseRoot의 기존 Scale 값을 저장
        if (horseRoot != null)
        {
            horseOriginalScale = horseRoot.localScale;
        }

        Inventory = new Inventory(inventorySlotCount);
        Equipment = new Equipment();

        playerHealth = GetComponent<PlayerHealth>();

        // 장비 장착시 이벤트
        Equipment.OnEquipmentChanged += UpdateDefenseFromEquipment;
        UpdateDefenseFromEquipment();

        InventoryController = new PlayerInventoryController(Inventory,Equipment,itemDatabase);
        WeaponController = new PlayerWeaponController(Inventory,Equipment,itemDatabase,defaultDamage);
    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        // 이동 입력은 한 번만 읽음
        ReadMovementInput();

        // 이동 방향에 따라 말 방향 변경
        UpdateFacingDirection();

        // 이동 애니메이션 갱신
        UpdateAnimation();

        // 마우스 방향 계산
        ReadAimDirection();

        // 스페이스바를 누르면 회피
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TryDodge();
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
        {
            moveInput.y += 1f;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            moveInput.y -= 1f;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            moveInput.x -= 1f;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            moveInput.x += 1f;
        }

        moveInput = moveInput.normalized;

        if (moveInput != Vector2.zero)
        {
            lastDirection = moveInput;
        }
    }

    private void UpdateFacingDirection()
    {
        if (horseRoot == null)
        {
            return;
        }

        // 좌우 입력이 없으면 현재 방향 유지
        if (Mathf.Abs(moveInput.x) < 0.01f)
        {
            return;
        }

        bool movingRight = moveInput.x > 0.01f;

        Vector3 scale = horseOriginalScale;
        float originalX = Mathf.Abs(horseOriginalScale.x);

        if (horseFacesRightByDefault)
        {
            // 원본이 오른쪽을 보는 이미지
            scale.x = movingRight ? originalX : -originalX;
        }
        else
        {
            // 원본이 왼쪽을 보는 이미지
            scale.x = movingRight ? -originalX : originalX;
        }

        horseRoot.localScale = scale;
    }

    private void UpdateAnimation()
    {
        if (animator == null)
        {
            return;
        }

        bool isMoving = moveInput != Vector2.zero;
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("1_Move", isMoving);

        if (isMoving)
        {
            animator.SetFloat("MoveX", lastDirection.x);
            animator.SetFloat("MoveY", lastDirection.y);
        }
    }

    private void TryDodge()
    {
        if (isDodging)
        {
            return;
        }

        if (currentDodgeCount <= 0)
        {
            return;
        }

        // 이동 중이면 이동 방향으로 회피
        // 멈춘 상태라면 마우스 방향으로 회피
        dodgeDirection =
            moveInput != Vector2.zero
                ? moveInput
                : aimDirection;

        currentDodgeCount--;

        StartCoroutine(DodgeCoroutine());

        if (dodgeRecoverCoroutine == null)
        {
            dodgeRecoverCoroutine = StartCoroutine(RecoverDodgeCharge());
        }
    }

    private IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        isInvincible = true;

        if (animator != null)
        {
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsDodging", true);
        }

        float timer = 0f;

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

        if (animator != null)
        {
            animator.SetBool("IsDodging", false);
        }
    }

    private void ReadAimDirection()
    {
        if (mainCamera == null || Mouse.current == null)
        {
            return;
        }

        Vector3 mouseWorldPosition =
            mainCamera.ScreenToWorldPoint(
                Mouse.current.position.ReadValue()
            );

        mouseWorldPosition.z = 0f;

        Vector2 direction =
            ((Vector2)mouseWorldPosition -
             (Vector2)transform.position).normalized;

        if (direction != Vector2.zero)
        {
            aimDirection = direction;
        }
    }

    // 회피 회복 루틴
    private IEnumerator RecoverDodgeCharge()
    {
        while (currentDodgeCount < maxDodgeCount)
        {
            yield return new WaitForSeconds(dodgeRecoverTime);

            currentDodgeCount++;
            currentDodgeCount = Mathf.Min(currentDodgeCount, maxDodgeCount);

            Debug.Log($"회피 회복: {currentDodgeCount} / {maxDodgeCount}");
        }

        dodgeRecoverCoroutine = null;
    }



    public bool RestoreDodge(int amount) // 회피 회복(포션용)
    {
        if (amount <= 0) return false;

        if (currentDodgeCount >= maxDodgeCount)
        {
            return false;
        }

        currentDodgeCount += amount;
        currentDodgeCount = Mathf.Min(currentDodgeCount, maxDodgeCount);

        Debug.Log($"회피 횟수 회복: {currentDodgeCount} / {maxDodgeCount}");

        return true;
    }

    // 장비 장착시 방어구에 대한 방어력 계산하여 방어력 정보 업데이트
    private void UpdateDefenseFromEquipment()
    {
        int newDefense = 0;

        if (Equipment.TryGetSlot(EquipmentSlotType.Armor, out EquipmentSlot armorSlot))
        {
            if (!armorSlot.IsEmpty)
            {
                ItemData itemData = itemDatabase.GetItem(armorSlot.ItemId);

                if (itemData is ArmorData armorData)
                {
                    newDefense = armorData.Defense;
                }
            }
        }

        playerHealth.SetDefense(newDefense);
    }
}
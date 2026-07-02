using UnityEngine;

/// <summary>
/// HUD UI를 관리하기 위한 컴포넌트 입니다.
/// </summary>
public class PlayerHUDController : MonoBehaviour
{
    [Header("플레이어 오브젝트 할당")]
    [SerializeField] private Player player;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("플레이어 HUD")]
    [SerializeField] private PlayerHpPanel hpPanel; // 체력 패널
    [SerializeField] private PlayerDodgePanel dodgePanel;   // 회피 패널
    [SerializeField] private AmmoPanel ammoPanel;   // 총알 개수 표시 패널
    [SerializeField] private IndicatorPanel indicatorPanel; // 플레이어가 피격 / 회복 시 표시 될 패널

    private PlayerWeaponController weaponController;
    private int curDodgeCount = -1;
    private int maxDodgeCount = -1;
    private int maxHp = 0;
    private int curHp = 0;
    private void Awake()
    {
        // 캐싱 확인
        if(hpPanel == null) hpPanel = FindAnyObjectByType<PlayerHpPanel>();
        if(dodgePanel == null) dodgePanel = FindAnyObjectByType<PlayerDodgePanel>();
        if (player == null) player = FindAnyObjectByType<Player>();
        if (playerHealth == null) playerHealth = FindAnyObjectByType<PlayerHealth>();
        if (ammoPanel == null) ammoPanel = FindAnyObjectByType<AmmoPanel>();
    }
    private void Start()
    {
        // UI 초기화 (항상 열려 있기 때문에 Start 사용)
        if(player != null && dodgePanel != null)
        {
            dodgePanel.MaxDodgeCountChanged(player.MaxDodgeCount);
            dodgePanel.DodgeUIUpdate(player.CurrentDodgeCount);
            curDodgeCount = player.CurrentDodgeCount;
            maxDodgeCount = player.MaxDodgeCount;
        }
        if(playerHealth != null && hpPanel != null)
        {
            curHp = playerHealth.CurrentHealth;
            maxHp = playerHealth.MaxHealth;
            hpPanel.Init(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        }
        if(player != null)
        {
            weaponController = player.WeaponController;
            if(weaponController != null)
            {
                UpdateAmmo();
                weaponController.OnAmmoChanged += UpdateAmmo;
            }

            if(player.Equipment != null)
            {
                player.Equipment.OnEquipmentChanged += UpdateAmmo;
            }
        }
    }
    private void OnDestroy()
    {
        if(weaponController != null)
        {
            weaponController.OnAmmoChanged -= UpdateAmmo;
        }
        if (player != null && player.Equipment != null)
        {
            player.Equipment.OnEquipmentChanged -= UpdateAmmo;
        }
    }
    private void Update()
    {
        if(playerHealth != null && maxHp != playerHealth.MaxHealth)
        {
            if(hpPanel != null)
                hpPanel.Init(playerHealth.CurrentHealth, playerHealth.MaxHealth);
            maxHp = playerHealth.MaxHealth;
        }
        if (playerHealth != null && curHp != playerHealth.CurrentHealth)
        {
            if(hpPanel != null)
                hpPanel.HpUIUpdate(playerHealth.CurrentHealth, playerHealth.MaxHealth);

            if(indicatorPanel != null)
            {
                if (playerHealth.CurrentHealth < curHp)
                    indicatorPanel.HitIndicator();  // 기존 체력 보다 줄어듦 => 피격 당한 상황
                else
                    indicatorPanel.HealIndicator(); // 기존 체력 보다 늘어남 => 회복 한 상황
            }
            curHp = playerHealth.CurrentHealth;
        }

        // 플레이어가 null이 아니면 dodgePanel에 갱신하는 함수를 호출
        if (player == null || dodgePanel == null) return;

        // 현재 회피 수가 동일하면 갱신하지 않음
        if (player.CurrentDodgeCount != curDodgeCount)
        {
            dodgePanel.DodgeUIUpdate(player.CurrentDodgeCount);
            curDodgeCount = player.CurrentDodgeCount;
        }
        if (player.MaxDodgeCount != maxDodgeCount)
        {
            dodgePanel.MaxDodgeCountChanged(player.MaxDodgeCount);
            maxDodgeCount = player.MaxDodgeCount;
        }
    }
    private void UpdateAmmo()
    {
        if (ammoPanel == null) return;

        if(weaponController  == null)
        {
            weaponController = player.WeaponController;
            if(weaponController == null)
            {
                return;
            }
        }

        WeaponData data = weaponController.GetEquippedWeapon();

        ammoPanel.PaintUI(data, weaponController.CurrentAmmo, weaponController.MaxAmmo);
    }
}

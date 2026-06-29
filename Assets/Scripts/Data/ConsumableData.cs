using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableData : ItemData
{
    // 소비 아이템
    // 타입(순수 회복, 회피 수치 회복)
    // 회복 값

    [Header("Consumable")]
    [SerializeField] private ConsumableType consumableType;
    [SerializeField] private int value;

    public ConsumableType ConsuabmeType => consumableType;
    public int Value => value;

    public override bool Use(Player player)
    {
        if (player == null) return false;

        switch (consumableType)
        {
            case ConsumableType.HealHp: // 체력 포션 사용
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth == null) return false;

                return playerHealth.Heal(value);

            case ConsumableType.RestoreDodge:
                // 나중에 회피 회복 처리
                break;

            default:
                return false;
        }
    }
}

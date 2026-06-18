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

    public override void Use(Player player)
    {
        // player.UseConsumable(this);
    }
}

using UnityEngine;

[CreateAssetMenu(menuName = "Item/Accessory")]
public class AccessoryData : ItemData
{
    [Header("Accessory Stat Bonus")]
    [SerializeField] private int maxHpBonus;

    public int MaxHpBonus => maxHpBonus;

    public override bool Use(Player player)
    {
        return false;
    }
}
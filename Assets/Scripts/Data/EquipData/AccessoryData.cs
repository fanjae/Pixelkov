using UnityEngine;

[CreateAssetMenu(menuName = "Item/Accessory")]
public class AccessoryData : ItemData
{
    [Header("Accessory Stat Bonus")]
    [SerializeField] private int maxHpBonus;
    [SerializeField] private int maxDodgeBonus;

    public int MaxHpBonus => maxHpBonus;
    public int MaxDodgeBonus => maxDodgeBonus;

    public override bool Use(Player player)
    {
        return false;
    }
}
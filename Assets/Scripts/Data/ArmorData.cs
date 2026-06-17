using UnityEngine;

[CreateAssetMenu(menuName = "Item/Armor")]
public class ArmorData : ItemData
{
    [Header("Armor")]
    [SerializeField] private int defense;
    [SerializeField] private int maxUpgradeCount;
    [SerializeField] private int upgradeCostPerLevel;

    public int Defense => defense;
    public int MaxUpgradeCount => maxUpgradeCount;
    public int UpGradeCostPerLevel => upgradeCostPerLevel;

    public override void Use(Player player)
    {
        // player.EquipArmor(this);
    }

    // 방어구 아이템 데이터
    // - 기본 방어력
    // - 최대 강화 횟수
    // - 강화당 코스트

}

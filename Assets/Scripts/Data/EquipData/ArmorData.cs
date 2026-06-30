using UnityEngine;

[CreateAssetMenu(menuName = "Item/Armor")]
public class ArmorData : ItemData
{
    [Header("Armor")]
    [SerializeField] private int defense;

    [Header("Upgrade")]
    [SerializeField] private int upgradeCost;
    [SerializeField] private int nextUpgradeItemId;

    public int Defense => defense;
    public int UpgradeCost => upgradeCost;
    public int NextUpgradeItemId => nextUpgradeItemId;
    public bool CanUpgrade => nextUpgradeItemId > 0;

    public override bool Use(Player player)
    {
        return false;
    }

    // 방어구 아이템 데이터
    // - 기본 방어력
    // - 강화 비용
    // - 강화시 나올 새로운 아이템 ID
}

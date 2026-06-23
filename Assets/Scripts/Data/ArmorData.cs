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

    public override void Use(Player player)
    {
        // player.EquipArmor(this);
        // 초기에는 이쪽에서 처리하려고 했으나, 컨트롤러 쪽에 고정되면서
        // 필요성이 모호해진 상태.
    }

    // 방어구 아이템 데이터
    // - 기본 방어력
    // - 강화 비용
    // - 강화시 나올 새로운 아이템 ID
}

using UnityEngine;

// 조합 재료의 보유 상태를 관리하는 데이터
public class CraftMaterialStatus
{
    public int ItemId { get; }
    public ItemData ItemData { get; }
    public int RequiredCount { get; }
    public int OwnedCount { get; }
    public int LackCount => Mathf.Max(0, RequiredCount - OwnedCount);
    public bool IsEnough => OwnedCount >= RequiredCount;

    public CraftMaterialStatus(int itemId,ItemData itemData,int requiredCount,int ownedCount)
    {
        ItemId = itemId;
        ItemData = itemData;
        RequiredCount = requiredCount;
        OwnedCount = ownedCount;
    }

}
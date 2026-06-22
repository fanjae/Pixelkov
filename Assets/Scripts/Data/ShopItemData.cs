using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemData", menuName = "Shop/Shop Item Data")]
public class ShopItemData : ScriptableObject
{
    [SerializeField] private int itemId;

    public int ItemId => itemId;

    // 상점에서 파는 아이템 1개 관련 정보
    // 실제 Item ID
}

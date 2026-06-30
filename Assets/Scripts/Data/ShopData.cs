using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "Shop/Shop Data")]
public class ShopData : ScriptableObject
{
    [SerializeField] private List<ShopItemData> shopItems;
    public IReadOnlyList<ShopItemData> ShopItems => shopItems;

    // 특정 상점에서 판매하는 아이템의 목록
}

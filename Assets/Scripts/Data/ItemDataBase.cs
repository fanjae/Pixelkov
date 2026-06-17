
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Item/Item Database")]
public class ItemDatabase : ScriptableObject
{
    // 전체 아이템 목록
    [SerializeField] private List<ItemData> items;

    // ItemId로 찾기 위한 Dictionary
    private Dictionary<int, ItemData> itemMap;

    // Items 리스트를 ItemId 기준 Dictionary로 변환
    public void Initialize()
    {
        itemMap = new Dictionary<int, ItemData>();

        foreach (ItemData item in items)
        {
            if (item == null) continue;

            itemMap[item.ItemId] = item;
        }
    }

    // ItemID에 해당하는 ItemData 반환
    public ItemData GetItem(int itemId)
    {
        // 아직 초기화 되지 않았다면 1회 초기화 진행
        if (itemMap == null) Initialize();

        itemMap.TryGetValue(itemId, out ItemData item);
        return item;
    }
}
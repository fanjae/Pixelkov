using System;

[Serializable]
public class InventorySlot
{
    public ItemInstance ItemInstance { get; private set; }
    public int Count { get; private set; }

    public int ItemId => ItemInstance?.ItemId ?? 0;
    public bool IsEmpty => ItemInstance == null || Count <= 0;


    public void SetItem(ItemData itemData, int count)
    {
        if (itemData == null) throw new ArgumentNullException(nameof(itemData));
        if (count <= 0) throw new ArgumentException("Item count >= 1");

        ItemInstance = new ItemInstance(itemData);
        Count = count;
    }

    public void AddCount(int amount) // 현재 수량에 아이템 추가
    {
        if (amount <= 0) return;

        Count += amount;
    }

    public void RemoveCount(int amount) // 현재 수량에 아이템 제거
    {
        if (amount <= 0) return;

        Count -= amount;

        if (Count <= 0) Clear();
    }

    public void Clear()
    {
        ItemInstance = null;
        Count = 0;
    }
}

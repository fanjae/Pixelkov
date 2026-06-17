using System;

[Serializable]
public class InventorySlot
{
    public int ItemId { get; private set; }
    public int Count { get; private set; }

    public bool IsEmpty => ItemId <= 0 || Count <= 0;


    // 빈 슬롯
    public InventorySlot()
    {
        Clear();
    }

    // 아이템 정보 포함 슬롯 생성
    public InventorySlot(int itemId, int count)
    {
        SetItem(itemId, count);
    }

    public void SetItem(int itemId, int count) // 슬롯에 아이템 설정
    {
        if (itemId <= 0) throw new ArgumentException("Item Id >= 1");
        if (count <= 0) throw new ArgumentException("Item count >= 1");

        ItemId = itemId;
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

    public void Clear() // 슬롯 상태 리셋
    {
        ItemId = 0;
        Count = 0;
    }
}

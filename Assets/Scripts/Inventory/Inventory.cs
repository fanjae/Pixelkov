using System;
using System.Collections.Generic;

public class Inventory
{
    private readonly List<InventorySlot> slots;
    public IReadOnlyList<InventorySlot> Slots => slots;

    public event Action OnInventoryChanged; // 인벤토리 변경에 대한 이벤트

    public Inventory(int slotCount) // 인벤토리 슬롯 셋팅
    {
        if (slotCount <= 0) throw new ArgumentException("Slot Count >= 1");

        // 고정 크기 인벤토리 전제로 생성 시 빈 슬롯을 미리 채움.
        slots = new List<InventorySlot>(slotCount);

        for(int i= 0; i <slotCount; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    public bool AddItem(ItemData itemData, int count = 1)
    {
        if (itemData == null || count <= 0) return false;

        int remainingCount = count;

        // 스택 가능한 아이템이면 기존 같은 아이템 슬롯부터 채움
        if(itemData.IsStackable)
        {
            remainingCount = AddToExistingStacks(itemData, remainingCount);
        }

        // 기존 슬롯에 다 못 넣은 수량은 빈 슬롯에 추가
        remainingCount = AddToEmptySlots(itemData, remainingCount);

        bool success = remainingCount == 0;

        // 아이템 전부 정상 추가되었으면 변경 이벤트 호출
        if (success)
            OnInventoryChanged?.Invoke();

        return success;
    }

    // 지정한 수 만큼 아이템 제거
    public bool RemoveItemAt(int slotIndex, int count = 1)
    {
        if (count <= 0) return false;

        // 잘못된 슬롯 처리
        if (!TryGetSlot(slotIndex, out InventorySlot slot)) return false;

        if (slot.IsEmpty) return false;

        // 슬롯에 있는 수량보다 많이 제거 불가
        if (slot.Count < count) return false;

        slot.RemoveCount(count);

        OnInventoryChanged?.Invoke();
        return true;
    }

    // 특정 ItemId를 가진 아이템을 인벤토리 전체에서 수량 만큼 제거
    public bool RemoveItem(int itemId, int count)
    {
        if (itemId <= 0 || count <= 0) return false;

        // 제거하기 전에 충분한 수량 보유 여부 확인
        if (!HasItem(itemId, count)) return false;

        int remainingCount = count;

        // 뒷쪽 슬롯부터 제거
        // 여러 슬롯에 나뉜 아이템은 순차적으로 소모
        for (int i = slots.Count - 1; i >= 0 && remainingCount > 0; i-- )
        {
            InventorySlot slot = slots[i];

            // 같은 ItemID를 가진 모든 슬롯 수량 합산
            if (slot.IsEmpty || slot.ItemId != itemId) continue;

            int removeCount = Math.Min(slot.Count, remainingCount);
            slot.RemoveCount(removeCount);
            remainingCount -= removeCount;
        }

        OnInventoryChanged?.Invoke();
        return true;
    }


    // 인벤토리에 특정 ItemID 아이템이 지정 수량 이상 존재하는지 체크
    public bool HasItem(int ItemId, int count)
    {
        if (ItemId <= 0 || count <= 0) return false;

        int totalCount = 0;

        foreach (InventorySlot slot in slots)
        {
            if(!slot.IsEmpty && slot.ItemId == ItemId)
            {
                totalCount += slot.Count;
            }
        }

        return totalCount >= count;
    }



    // 슬롯 인덱스로 슬롯을 가져옴
    public bool TryGetSlot(int slotIndex, out InventorySlot slot)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count) // 잘못 인덱스 처리
        {
            slot = null;
            return false;
        }

        slot = slots[slotIndex];
        return true;
    }


    // 같은 Item에 가진 기존 슬롯에 가능한 만큼 수량을 추가하고 남은 수량을 반환
    private int AddToExistingStacks(ItemData itemData, int count)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.IsEmpty) continue;

            if (slot.ItemId != itemData.ItemId) continue;

            // 기존 슬롯에 가능한 수 만큼 수량을 추가.
            int availableCount = itemData.MaxStackCount - slot.Count;

            if (availableCount <= 0) continue;

            // 기존 슬롯에 가능한 수 vs 아이템의 개수 비교
            int addCount = Math.Min(availableCount, count);
            slot.AddCount(addCount);
            count -= addCount;

            if (count <= 0) break;
        }
        return count;
    }


    // 빈 슬롯에 아이템 추가
    private int AddToEmptySlots(ItemData itemData, int count)
    {
        foreach  (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty) continue;

            // 스택 가능한 아이템은 최대 스택 수 만큼 처리하고 스택 불가능한 아이템은 슬롯당 1개씩 추가
            int addCount = itemData.IsStackable ? Math.Min(itemData.MaxStackCount, count) : 1;

            slot.SetItem(itemData.ItemId, addCount);
            count -= addCount;

            if (count <= 0) break;
        }

        // 남은 수량 반환
        return count;
    }

}

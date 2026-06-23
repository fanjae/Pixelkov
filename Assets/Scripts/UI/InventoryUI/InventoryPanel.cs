using System;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [Header("아이템 슬롯")]
    [SerializeField] private SlotUI[] slotUIs;  // 인벤토리에 있는 슬롯 오브젝트들
    private Inventory inventory;                // 인벤토리 데이터를 받을 변수

    private void Awake()
    {
        for(int i = 0; i < slotUIs.Length; i++)
        {
            slotUIs[i].SetIndex(i);
        }
    }

    /// <summary>
    /// 각 슬롯마다 갖고있는 Action에 메서드를 연결합니다.
    /// </summary>
    public void AllocateSlotEvent(Action<int> enter, Action exit, Action<int> equip)
    {
        foreach(var slot in slotUIs)
        {
            slot.OnSlotEnter += enter;
            slot.OnSlotExit += exit;
            slot.OnEquip += equip;
        }
    }
    /// <summary>
    /// 슬롯에 할당했던 메서드들 해제
    /// </summary>
    public void ReleaseSlotEvent(Action<int> enter, Action exit, Action<int> equip)
    {
        foreach(var slot in slotUIs)
        {
            slot.OnSlotEnter -= enter;
            slot.OnSlotExit -= exit;
            slot.OnEquip -= equip;
        }
    }

    public void AllocateSell(Func<int, int, bool> sell)
    {

        foreach(var slot in slotUIs)
        {
            slot.OnSell += sell;
        }
    }
    public void ReleaseSell(Func<int, int, bool> sell)
    {
        foreach(var slot in slotUIs)
        {
            slot.OnSell -= sell;
        }
    }

    /// <summary>
    /// 인벤토리를 제공받는 메서드.
    /// </summary>
    public void AllocateInventory(Inventory newInvetory)
    {
        inventory = newInvetory;
    }

    /// <summary>
    /// 모든 슬롯을 다시 그리는 메서드
    /// </summary>
    public void PaintInventoryAll()
    {
        for(int i = 0; i < inventory.Slots.Count; i++)
        {
            PaintSlotUI(i);
        }
    }

    private void PaintSlotUI(int index)
    {
        // 인벤토리를 받지 못한 경우
        if (inventory == null) return;

        // index가 UI상의 슬롯의 개수보다 크거나 인벤토리의 슬롯의 개수보다 큰 경우
        if (index >= slotUIs.Length || index >= inventory.Slots.Count) return;

        // 선택한 인벤토리의 슬롯이 할당되지 않은 경우
        if (inventory.Slots[index] == null) return;

        // index 번째의 슬롯 정보를 받아서 UI에 전달
        InventorySlot curSlotInfo = inventory.Slots[index];
        if(curSlotInfo != null)
            slotUIs[index].SetSlotInfo(curSlotInfo);
    }
}

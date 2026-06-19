using System;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
    [Header("아이템 슬롯")]
    [SerializeField] private EquipSlotUI[] equipSlotUIs;  // 장비창에 있는 슬롯 오브젝트들
    private Equipment equipment;                // 장비창의 데이터를 받을 변수

    public void AllocateSlotEvent(Action<EquipmentSlotType> unEquip)
    {
        foreach(var slot in equipSlotUIs)
        {
            slot.OnUnEquip += unEquip;
        }
    }
    public void ReleaseSlotEvent(Action<EquipmentSlotType> unEquip)
    {
        foreach(var slot in equipSlotUIs)
        {
            slot.OnUnEquip -= unEquip;
        }
    }
    /// <summary>
    /// 장비 정보를 제공받는 메서드.
    /// </summary>
    public void AllocateEquipment(Equipment newEquipment)
    {
        equipment = newEquipment;
    }

    /// <summary>
    /// 모든 슬롯을 다시 그리는 메서드
    /// </summary>
    public void PaintEquipmentAll()
    {
        foreach(var slot in equipment.Slots)
        {
            PaintSlotUI(slot.Key, slot.Value);
        }
    }

    private void PaintSlotUI(EquipmentSlotType type, EquipmentSlot slot)
    {
        // 장비 정보를 받지 못한 경우
        if (equipment == null) return;

        foreach(EquipSlotUI equipSlot in equipSlotUIs)
        {
            if (equipSlot == null) return;

            if(equipSlot.SlotType == type)
            {
                equipSlot.SetSlotInfo(slot);
                break;
            }
        }
    }
}

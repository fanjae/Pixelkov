using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeSlot : MonoBehaviour, IPointerClickHandler
{
    public Action ReleaseEvent;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.clickCount >= 2)
        {
            ReleaseEvent?.Invoke();
            eventData.clickCount = 0;
        }
    }
}

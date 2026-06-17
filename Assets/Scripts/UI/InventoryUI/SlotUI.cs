using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject subButton;
    private Vector3 originScale;

    private void Awake()
    {
        originScale = transform.localScale;
    }
    
    //
    public void PaintSlot(Sprite icon, int count)
    {
        iconImage.sprite = icon;
        if (count <= 1) countText.text = "";
        else countText.text = count.ToString();
    }

    #region 마우스 이벤트
    // 슬롯에 마우스 올리면 확대/복구
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(originScale * 1.1f, 0.15f).SetEase(Ease.OutQuad);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(originScale, 0.15f).SetEase(Ease.OutQuad);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // 우클릭 시 서브 버튼 활성화/비활성화
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (subButton == null) return;
            subButton.SetActive(!subButton.activeSelf);
        }
    }
    #endregion
}

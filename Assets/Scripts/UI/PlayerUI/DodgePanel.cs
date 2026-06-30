using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DodgePanel : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    /// <summary>
    /// 플레이어의 회피 게이지를 반영해서 갱신하는 메서드
    /// </summary>
    public void FillUpdate(float amount)
    {
        //fillImage.fillAmount = amount;
        fillImage.DOFillAmount(amount, 0.1f).SetEase(Ease.OutQuad);
    }
}

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DodgePanel : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    private Color defaultColor = Color.cyan;
    private Color ActiveColor = Color.blue;

    private void Awake()
    {
        if(fillImage != null)
        {
            defaultColor = fillImage.color;
        }
    }

    /// <summary>
    /// 플레이어의 회피 게이지를 반영해서 갱신하는 메서드
    /// </summary>
    public void FillUpdate(float amount)
    {
        fillImage.fillAmount = amount;
        if(amount < 0.5f) fillImage.color = defaultColor;
        else fillImage.color = ActiveColor;
    }
}

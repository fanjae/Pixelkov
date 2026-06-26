using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpPanel : MonoBehaviour
{
    [SerializeField] private Image fillImage;   // 체력바
    [SerializeField] private TextMeshProUGUI hpText;    // 체력 수치 표시

    /// <summary>
    /// 게임 시작시 플레이어 HP기반으로 초기화 해주는 메서드입니다.
    /// </summary>
    /// <param name="currentHp"></param>
    /// <param name="maxHp"></param>
    public void Init(int currentHp, int maxHp)
    {
        float ratio = (float)currentHp / maxHp;
        fillImage.fillAmount = ratio;
        hpText.text = $"{currentHp} / {maxHp}";
    }

    /// <summary>
    /// 주어지는 체력을 기반으로 HpUI를 갱신하는 메서드
    /// </summary>
    /// <param name="currentHp">현재 체력</param>
    /// <param name="maxHp">최대 체력</param>
    public void HpUIUpdate(int currentHp, int maxHp)
    {
        float ratio = (float)currentHp / maxHp; // 체력 비율을 구해서 DOTween을 사용해 fillAmount 조정
        fillImage.DOKill();
        fillImage.DOFillAmount(ratio, 0.05f).SetEase(Ease.OutQuad);
        hpText.text = $"{currentHp} / {maxHp}";
    }
}

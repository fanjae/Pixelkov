using DG.Tweening;
using TMPro;
using UnityEngine;
using MagicPigGames;

public class PlayerHpPanel : MonoBehaviour
{
    [SerializeField] private ProgressBarInspectorTest hpBar;   // 체력바
    [SerializeField] private TextMeshProUGUI hpText;    // 체력 수치 표시

    Tween tween;
    /// <summary>
    /// 게임 시작시 플레이어 HP기반으로 초기화 해주는 메서드입니다.
    /// </summary>
    public void Init(int currentHp, int maxHp)
    {
        float ratio = (float)currentHp / maxHp;
        hpBar.progress = ratio;
        hpText.text = $"{currentHp} / {maxHp}";
    }

    /// <summary>
    /// 주어지는 체력을 기반으로 HpUI를 갱신하는 메서드
    /// </summary>
    /// <param name="currentHp">현재 체력</param>
    /// <param name="maxHp">최대 체력</param>
    public void HpUIUpdate(int currentHp, int maxHp)
    {
        if(tween != null)
            tween.Complete();
        float ratio = (float)currentHp / maxHp; // 체력 비율을 구해서 DOTween을 사용해 fillAmount 조정
        tween = DOTween.To(() => hpBar.progress, amount => hpBar.progress = amount, ratio, 0.2f);
        tween.Play();
        hpText.text = $"{currentHp} / {maxHp}";
    }
}

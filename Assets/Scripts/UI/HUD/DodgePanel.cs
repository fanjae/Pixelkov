using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using MagicPigGames;

public class DodgePanel : MonoBehaviour
{

    [SerializeField] private ProgressBarInspectorTest hpBar;   // 체력바

    Tween tween;
    /// <summary>
    /// 플레이어의 회피 게이지를 반영해서 갱신하는 메서드
    /// </summary>
    public void FillUpdate(float amount)
    {
        if(tween != null)
            tween.Complete();
        tween = DOTween.To(() => hpBar.progress, progress => hpBar.progress = progress, amount, 0.2f);
        tween.Play();
    }
}

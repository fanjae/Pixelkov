using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorPanel : MonoBehaviour
{
    [SerializeField] private Image outline;

    private void Awake()
    {
        if(outline == null) outline = GetComponent<Image>();
    }

    public void HitIndicator()
    {
        if (outline == null) return;
        DOTween.Kill(outline);
        outline.color = new Color(1.0f, 0.0f, 0.0f, 0.75f);
        outline.DOColor(new Color(1.0f, 0.0f, 0.0f, 0.0f), 0.3f);
    }
    public void HealIndicator()
    {
        if (outline == null) return;
        DOTween.Kill(outline);
        outline.color = new Color(0.0f, 1.0f, 0.0f, 0.75f);
        outline.DOColor(new Color(0.0f, 1.0f, 0.0f, 0.0f), 0.3f);
    }
}

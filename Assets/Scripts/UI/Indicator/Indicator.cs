using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    [Header("외부 참조 컴포넌트")]
    [SerializeField] private Transform playerTransform;
    [Header("내부 참조 컴포넌트")]
    [SerializeField] private Transform indicatorCanvas;
    [SerializeField] private Image indicator;
    [SerializeField] private RectTransform damageView;

    [SerializeField] private float angleOffset = -45f;
    
    private Camera cam;
    private Stack<Image> indicatorPool = new Stack<Image>();
    private Stack<RectTransform> damageViewPool = new Stack<RectTransform>();

    private void Awake()
    {
        cam = Camera.main;
        DamageHandler.OnPlayerAttack += ShowDamageText;
        DamageHandler.OnEnemyAttack += ShowDamageText;
        DamageHandler.OnEnemyAttack += ShowIndicator;
    }
    private void OnDestroy()
    {
        DamageHandler.OnPlayerAttack -= ShowDamageText;
        DamageHandler.OnEnemyAttack -= ShowDamageText;
        DamageHandler.OnEnemyAttack -= ShowIndicator;
    }

    private void ShowIndicator(Vector2 targetVector, int damage)
    {
        StartCoroutine(IndicatorEnumerator(targetVector));
    }
    private void ShowDamageText(Vector2 targetVector, int damage)
    {
        StartCoroutine(DamageTextEnumerator(targetVector, damage));
    }
    private IEnumerator IndicatorEnumerator(Vector2 targetVector)
    {
        Vector2 direction = (Vector2)playerTransform.position - targetVector;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
        Image indi;
        if(indicatorPool.Count > 0)
        {
            indi = indicatorPool.Pop();
            indi.rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
            indi.gameObject.SetActive(true);
        }
        else
        {
            indi = Instantiate(indicator, Vector2.zero, Quaternion.Euler(0.0f, 0.0f, angle), indicatorCanvas);
            indi.rectTransform.anchoredPosition = Vector2.zero;
        }
        if(indi == null) yield break;

        indi.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        Tween tween = indi.DOFade(0.0f, 0.3f);
        yield return tween.WaitForCompletion();
        indi.gameObject.SetActive(false);
        indicatorPool.Push(indi);
    }
    private IEnumerator DamageTextEnumerator(Vector2 targetVector, int damage)
    {
        RectTransform view;
        if(damageViewPool.Count > 0)
        {
            view = damageViewPool.Pop();
            view.position = targetVector;
            view.gameObject.SetActive(true);
        }
        else
        {
            view = Instantiate(damageView, Vector2.zero, Quaternion.identity, transform);
            view.position = targetVector;
        }
        if (view == null) yield break;

        TextMeshProUGUI damageText = view.GetComponentInChildren<TextMeshProUGUI>();
        if (damageText == null) yield break;

        damageText.text = damage.ToString();
        damageText.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        Sequence sequence = DOTween.Sequence();
        sequence.Join(damageText.DOFade(0.0f, 1f));
        sequence.Join(view.DOMoveY(targetVector.y + 1f, 1f));
        yield return sequence.WaitForCompletion();
        view.gameObject.SetActive(false);
        damageViewPool.Push(view);
    }
}

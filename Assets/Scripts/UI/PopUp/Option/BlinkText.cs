using DG.Tweening;
using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float duration = 1.0f;
    private Sequence sequence;
    private void Start()
    {
        if(text  == null)
        {
            if(!TryGetComponent(out text))
                return;
        }
        sequence = DOTween.Sequence();
        sequence.Append(text.DOColor(new Color(1.0f, 1.0f, 1.0f, 0.5f), duration));
        sequence.Append(text.DOColor(new Color(1.0f, 1.0f, 1.0f, 1.0f), duration));
        sequence.SetLoops(-1);
    }
    private void OnDestroy()
    {
        if(sequence != null)
            sequence.Kill();
    }
}

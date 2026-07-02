using DG.Tweening;
using UnityEngine;

public class InteractCanvas : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 targetPos;
    [SerializeField] private Vector3 offset;

    private Sequence sequence;

    private void Awake()
    {
        startPos = transform.position;
        targetPos = transform.position + offset;
    }
    private void OnEnable()
    {
        //DOTween.Kill(transform);
        transform.position = startPos;
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(targetPos, 0.5f)).Append(transform.DOMove(startPos, 1f));
        sequence.SetLoops(-1);
    }
    private void OnDisable()
    {
        if (sequence != null)
            sequence.Kill();
    }
}

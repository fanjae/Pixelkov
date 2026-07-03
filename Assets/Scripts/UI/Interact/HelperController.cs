using DG.Tweening;
using UnityEngine;

public class HelperController : MonoBehaviour
{
    [SerializeField] private bool isOpen = true;
    [SerializeField] private float ypos = 250f;

    private Vector2 openPos = new Vector2(-400f, 0f);
    private Vector2 closePos = new Vector2(0f, 0f);

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (isOpen) rectTransform.anchoredPosition = openPos + new Vector2(0.0f, ypos);
        else rectTransform.anchoredPosition = closePos + new Vector2(0.0f, ypos);
    }
    public void SwitchHelper()
    {
        DOTween.Kill(rectTransform);
        Vector2 targetPos = new Vector2(0.0f, ypos);
        if (isOpen)
        {
            targetPos += closePos;
        }
        else
        {
            targetPos += openPos;
        }

        rectTransform.DOAnchorPos(targetPos, 0.3f);
        isOpen = !isOpen;
    }
}

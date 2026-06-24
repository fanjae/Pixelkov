using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        // 씬 들어오면 FadeIn
        StartCoroutine(FadeIn());
        // FadeOut을 이벤트에 구독해서 씬 전환할 때 FadeOut
        if(SceneLoadManager.Instance != null)
            SceneLoadManager.Instance.FadeEvent += FadeOut;
    }
    private void OnDestroy()
    {
        if(SceneLoadManager.Instance != null)
            SceneLoadManager.Instance.FadeEvent -= FadeOut;
    }

    private YieldInstruction FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        // DOTween과 코루틴을 연계해서 씬 전환 타이밍을 조절
        Tween tween = fadeImage.DOFade(1.0f, fadeDuration);
        return tween.WaitForCompletion();
    }
    private IEnumerator FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        Tween tween = fadeImage.DOFade(0.0f, fadeDuration);
        yield return tween.WaitForCompletion();
        fadeImage.gameObject.SetActive(false);
    }
}

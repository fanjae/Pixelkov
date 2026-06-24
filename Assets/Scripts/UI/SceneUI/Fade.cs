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
        StartCoroutine(FadeOut());
        SceneLoadManager.Instance.FadeEvent += FadeIn;
    }
    private void OnDestroy()
    {
        SceneLoadManager.Instance.FadeEvent -= FadeIn;
    }

    private YieldInstruction FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        Tween tween = fadeImage.DOFade(1.0f, fadeDuration);
        return tween.WaitForCompletion();
    }
    private IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        Tween tween = fadeImage.DOFade(0.0f, fadeDuration);
        yield return tween.WaitForCompletion();
        fadeImage.gameObject.SetActive(false);
    }
}

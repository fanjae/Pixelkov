using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MapTeleportZone : MonoBehaviour
{
    [Header("텔레포트 위치")]
    [SerializeField] private Transform targetPoint;

    [Header("페이드 UI")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;

    private bool isTeleporting;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTeleporting) return;

        // Player 만 텔레포트
        if (!other.CompareTag("Player")) return;
        if (targetPoint == null) return;

        StartCoroutine(TeleportRoutine(other));
    }

    private IEnumerator TeleportRoutine(Collider2D other)
    {
        isTeleporting = true;

        Player player = other.GetComponent<Player>();
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

        // 포탈 이용 중 플레이어 입력/이동 정지
        if (player != null) player.enabled = false;

        if (rb != null) rb.linearVelocity = Vector2.zero;

        // 화면 어둡게
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0f, 0f, 0f, 0f);

            Tween fadeOutTween = fadeImage.DOFade(1f, fadeDuration);
            yield return fadeOutTween.WaitForCompletion();
        }

        // 실제 텔레포트 처리
        if (rb != null)
        {
            rb.position = targetPoint.position;
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            // Rigidbody 없으면 Position 이동 방식
            other.transform.position = targetPoint.position;
        }

        // 포탈 종료 후 플레이어 입력/이동 복구
        if (player != null)
            player.enabled = true;

        // 화면 다시 밝게
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0f, 0f, 0f, 1f);

            Tween fadeInTween = fadeImage.DOFade(0f, fadeDuration);
            yield return fadeInTween.WaitForCompletion();

            fadeImage.gameObject.SetActive(false);
        }

        isTeleporting = false;
    }
}
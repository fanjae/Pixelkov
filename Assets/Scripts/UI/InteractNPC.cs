using AllIn1SpriteShader;
using UnityEngine;

public class InteractNPC : MonoBehaviour
{
    [SerializeField] private GameObject interactPanel;
    [SerializeField] private GameObject guideCanvas;

    /// <summary>
    /// 상점 온/오프 메서드
    /// </summary>
    public void SwitchPanel()
    {
        if(interactPanel != null)
            interactPanel.SetActive(!interactPanel.activeSelf);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 콜라이더 내로 진입 시 상점 기능 구독
        if(collision.CompareTag("Player"))
        {
            InputUIController.ShopAction += SwitchPanel;
            // 가이드 아이콘 활성화
            if(guideCanvas != null)
            {
                guideCanvas.SetActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 플레이어가 떠나면 상점 기능 취소
        if(collision.CompareTag("Player"))
        {
            InputUIController.ShopAction -= SwitchPanel;
            // 가이드 아이콘 비활성화
            if(guideCanvas != null)
            {
                guideCanvas.SetActive(false);
            }
            if (interactPanel != null)
            {
                interactPanel.SetActive(false);
            }
        }
    }
    private void OnDestroy()
    {
        // 파괴시에는 구독여부 상관없이 취소
        InputUIController.ShopAction -= SwitchPanel;
    }
}

using AllIn1SpriteShader;
using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    [SerializeField] private GameObject Shop;

    // Shader를 위한 메테리얼
    private Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    /// <summary>
    /// 상점 온/오프 메서드
    /// </summary>
    public void SwitchShop()
    {
        if(Shop != null)
            Shop.SetActive(!Shop.activeSelf);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 콜라이더 내로 진입 시 상점 기능 구독
        if(collision.CompareTag("Player"))
        {
            InputUIController.ShopAction += SwitchShop;
            // Outline 활성화
            if(material != null)
            {
                material.EnableKeyword("OUTBASE_ON");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 플레이어가 떠나면 상점 기능 취소
        if(collision.CompareTag("Player"))
        {
            InputUIController.ShopAction -= SwitchShop;
            // Outline 비활성화
            if(material != null)
            {
                material.DisableKeyword("OUTBASE_ON");
            }
        }
    }
    private void OnDestroy()
    {
        // 파괴시에는 구독여부 상관없이 취소
        InputUIController.ShopAction -= SwitchShop;
    }
}

using AllIn1SpriteShader;
using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    [SerializeField] private GameObject Shop;
    private Material material;
    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }
    public void SwitchShop()
    {
        if(Shop != null)
            Shop.SetActive(!Shop.activeSelf);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            InputUIController.ShopAction += SwitchShop;
            if(material != null)
            {
                material.EnableKeyword("OUTBASE_ON");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            InputUIController.ShopAction -= SwitchShop;
            if(material != null)
            {
                material.DisableKeyword("OUTBASE_ON");
            }
        }
    }
    private void OnDestroy()
    {
        InputUIController.ShopAction -= SwitchShop;
    }
}

using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    [SerializeField] private GameObject Shop;
    public void SwitchShop()
    {
        Shop?.SetActive(!Shop.activeSelf);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            InputUIController.ShopAction += SwitchShop;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            InputUIController.ShopAction -= SwitchShop;
        }
    }
    private void OnDestroy()
    {
        InputUIController.ShopAction -= SwitchShop;
    }
}

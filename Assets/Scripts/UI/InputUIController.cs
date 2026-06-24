using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputUIController : MonoBehaviour
{
    // ShopNPC에서 상점 온/오프 기능을 할당해줄 Action
    public static Action ShopAction;

    // 온/오프 해줄 UIs
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject menuUI;

    private InputAction InputInventory;
    private InputAction InputMenu;
    private InputAction InputShop;

    private void Awake()
    {
        InputInventory = InputSystem.actions.FindAction("Inventory");
        InputMenu = InputSystem.actions.FindAction("Menu");
        InputShop = InputSystem.actions.FindAction("Shop");
    }

    void Update()
    {
        // 주석 위치에 플레이어 사망 상태에 따라 입력을 막는 로직 추가 예정

        // 인벤토리 Key : I
        if(InputInventory.WasPerformedThisFrame() && inventoryUI != null)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
        // 메뉴 Key : Esc
        if(InputMenu.WasPerformedThisFrame() && menuUI != null)
        {
            menuUI.SetActive(!menuUI.activeSelf);
        }
        // 상점 Key : E
        if(InputShop.WasCompletedThisFrame())
        {
            ShopAction?.Invoke();
        }
    }
}

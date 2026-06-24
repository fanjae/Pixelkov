using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputUIController : MonoBehaviour
{
    public static Action ShopAction;

    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject menuUI;

    private InputAction InputInventory;
    private InputAction InputMenu;
    private InputAction InputShop;

    private void Awake()
    {
        InputInventory = InputSystem.actions.FindAction("Inventory");
        InputMenu = InputSystem.actions.FindAction("Menu");
        InputShop = InputSystem.actions.FindAction("Shop"); // 기본으로 존재하는 Interact Action 재활용
    }

    void Update()
    {
        if(InputInventory.WasPerformedThisFrame() && inventoryUI != null)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
        if(InputMenu.WasPerformedThisFrame() && menuUI != null)
        {
            menuUI.SetActive(!menuUI.activeSelf);
        }
        if(InputShop.WasCompletedThisFrame())
        {
            ShopAction?.Invoke();
        }
    }
}

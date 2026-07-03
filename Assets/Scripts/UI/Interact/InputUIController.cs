using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputUIController : MonoBehaviour
{
    
    // 상호작용 기능 메서드를 제공 받는 Action
    public static Action InteractAction;
    public static LinkedList<GameObject> PopUpOrder = new LinkedList<GameObject>();

    // 온/오프 해줄 UIs
    [Header("외부 참조 컴포넌트")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("캔버스 내 참조 컴포넌트")]
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private HelperController helper;

    private InputAction InputInventory;
    private InputAction InputMenu;
    private InputAction InputShop;

    private void Awake()
    {
        InputInventory = InputSystem.actions.FindAction("Inventory");
        InputMenu = InputSystem.actions.FindAction("Menu");
        InputShop = InputSystem.actions.FindAction("Shop");

        if(playerHealth == null) playerHealth = FindAnyObjectByType<PlayerHealth>();
    }
    private void OnDestroy()
    {
        PopUpOrder.Clear();
    }

    void Update()
    {
        // 플레이어 사망 상태에 따라 입력을 막는 로직
        if(playerHealth != null && playerHealth.IsDead)
        {
            // 열려있는 팝업창 모두 닫기
            if (menuUI != null && menuUI.activeSelf) menuUI.SetActive(false);
            if(PopUpOrder.Count > 0)
            {
                foreach (GameObject order in PopUpOrder)
                {
                    order.SetActive(false);
                }
                PopUpOrder.Clear();
            }
            return;
        }

        // 인벤토리 Key : I
        if(InputInventory.WasPerformedThisFrame() && inventoryUI != null)
        {
            // 메뉴창이 열려있을 때는 상호작용 x
            // +) 메뉴창이 열려있을 때는 열려있는 창이 없다는 것을 보장 중
            if (menuUI != null && menuUI.activeSelf) return;

            // UI 활성화/비활성화
            inventoryUI.SetActive(!inventoryUI.activeSelf);

            // 활성화 시 LinkedList에 추가
            if (inventoryUI.activeSelf)
            {
                PopUpOrder.AddLast(inventoryUI);
            }
            // 비활성화 시 LinkedList에서 제거
            else
            {
                PopUpOrder.Remove(inventoryUI);
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.Play(SFXType.PopUp);
            }
        }
        // 메뉴 Key : Esc
        if(InputMenu.WasPerformedThisFrame() && menuUI != null)
        {
            // 열려있는 창이 있으면 해당 창을 우선 닫기
            if(PopUpOrder.Count > 0)
            {
                // 마지막으로 열었던 순서대로 닫기
                LinkedListNode<GameObject> lastPopUp = PopUpOrder.Last;
                lastPopUp.Value.gameObject.SetActive(false);
                PopUpOrder.RemoveLast();
            }
            // 없으면 메뉴창을 활성화/비활성화
            else
            {
                menuUI.SetActive(!menuUI.activeSelf);
            }
        }
        // 상점 Key : E
        if(InputShop.WasCompletedThisFrame())
        {
            // 메뉴창이 열려있을 때는 상호작용 x
            // +) 메뉴창이 열려있을 때는 열려있는 창이 없다는 것을 보장 중
            if (menuUI != null && menuUI.activeSelf) return;

            InteractAction?.Invoke();
        }
        if (Keyboard.current.tabKey.wasPressedThisFrame && helper != null)
        {
            helper.SwitchHelper();
        }
    }
}

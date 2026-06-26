using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    // 외부에 마우스가 UI위에 있는지 알림
    public static bool HoverUI = false;

    [Header("외부 참조 컴포넌트")]
    [SerializeField] private Camera cam;

    [Header("캔버스 내 참조 컴포넌트")]
    [SerializeField] private Image cursorImage;
    [SerializeField] private Sprite crossHair;
    [SerializeField] private Sprite mouseCursor;

    private void OnEnable()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
        Cursor.visible = false;
    }
    private void OnDisable()
    {
        Cursor.visible = true;
    }

    private void Update()
    {
        if (cursorImage == null) return;

        cursorImage.transform.position = Mouse.current.position.ReadValue();
        if(EventSystem.current.IsPointerOverGameObject())
        {
            // 마우스가 UI 위에 있는 상태
            if(mouseCursor != null)
            {
                cursorImage.sprite = mouseCursor;
            }
            HoverUI = true;
        }
        else
        {
            if(crossHair != null)
            {
                cursorImage.sprite = crossHair;
            }
            HoverUI = false;
        }
    }
}

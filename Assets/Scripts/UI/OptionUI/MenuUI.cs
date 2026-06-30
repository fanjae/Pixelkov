using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject VolumnPanel;

    private void OnDisable()
    {
        if(MenuPanel != null)
            MenuPanel.SetActive(true);
        if(VolumnPanel != null)
            VolumnPanel.SetActive(false);
    }
}

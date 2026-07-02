using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubButton : MonoBehaviour
{
    [SerializeField] private Button useButton;
    [SerializeField] private Button sellButton;
    [SerializeField] private Button upgradeButton;

    public void UpdateUI(string nextText, bool isUse, bool isSell, bool isUpgrade)
    {
        if(useButton != null)
        {
            TextMeshProUGUI buttonText = useButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null) buttonText.text = nextText;
            useButton.gameObject.SetActive(isUse);
        }
        if(sellButton != null) sellButton.gameObject.SetActive(isSell);
        if(upgradeButton != null) upgradeButton.gameObject.SetActive(isUpgrade);
    }
}

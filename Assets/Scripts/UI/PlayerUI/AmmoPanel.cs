using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoPanel : MonoBehaviour
{
    [SerializeField] private Image ammoIcon;
    [SerializeField] private TextMeshProUGUI infText;
    [SerializeField] private TextMeshProUGUI curText;
    [SerializeField] private TextMeshProUGUI maxText;
    [SerializeField] private Sprite defaultIcon;
    [SerializeField] private Color defaultColor;

    public void PaintUI(WeaponData weaponData, int curAmmo, int maxAmmo)
    {
        if(weaponData == null)
        {
            ammoIcon.sprite = defaultIcon;
            ammoIcon.color = defaultColor;
        }
        else
        {
            ammoIcon.sprite = weaponData.Icon;
            ammoIcon.color = Color.white;
        }

        if (maxAmmo == 0)
        {
            infText.gameObject.SetActive(true);
            curText.gameObject.SetActive(false);
            maxText.gameObject.SetActive(false);
        }
        else
        {
            infText.gameObject.SetActive(false);
            curText.gameObject.SetActive(true);
            maxText.gameObject.SetActive(true);
            curText.text = $"{curAmmo}";
            maxText.text = $"/{maxAmmo}";

            if(maxAmmo != 0 && curAmmo / maxAmmo <= 0.2f)
            {
                curText.color = Color.red;
            }
            else if(maxAmmo != 0 && curAmmo / maxAmmo <= 0.4f)
            {
                curText.color = Color.yellow;
            }
            else
            {
                curText.color = Color.white;
            }
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoPanel : MonoBehaviour
{
    [SerializeField] private Image ammoIcon;
    [SerializeField] private TextMeshProUGUI ammoText;
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
            ammoText.text = "∞";
        }
        else
        {
            ammoText.text = $"{curAmmo} / {maxAmmo}";
        }
    }
}

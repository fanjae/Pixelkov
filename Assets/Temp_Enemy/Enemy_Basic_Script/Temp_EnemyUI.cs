using UnityEngine;
using UnityEngine.UI;

public class Temp_EnemyUI : MonoBehaviour
{
    [SerializeField] private Image hpFillImage;
    private int maxHp;

    //초기 HP 셋팅
    public void Initialize(int maxHealth)
    {
        maxHp = maxHealth;

        if (hpFillImage != null) hpFillImage.fillAmount = 1.0f;
    }

    //Damage 후 HP셋팅
    public void SetHP(int currentHp)
    {
        if (hpFillImage == null || maxHp <= 0) return;

        hpFillImage.fillAmount = Mathf.Clamp01((float)currentHp / maxHp);
    }
}

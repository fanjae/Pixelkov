using UnityEngine;
using UnityEngine.UI;
namespace Enemy_Player
{

    public class Enemy_PlayerUI : MonoBehaviour
    {
        [SerializeField] private Image hpFillImage;
        private int maxHp;

        //초기 HP 셋팅
        public void Initialize(int maxHealth)
        {
            maxHp = maxHealth;
            hpFillImage.fillAmount = 1.0f;
        }
        //Damage 후 HP셋팅
        public void SetHP(int currentHp)
        {
            hpFillImage.fillAmount = (float)currentHp / maxHp;
        }
    }
}
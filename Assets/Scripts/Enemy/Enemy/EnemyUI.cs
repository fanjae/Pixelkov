using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class EnemyUI : MonoBehaviour
    {
        
        [SerializeField] private Image hpFillImage;
        [SerializeField] private Transform target; 
        private int maxHp;

        private void Update()
        {
            if (target == null || hpFillImage == null) return;
            Quaternion quaternion = target.rotation;
            if (quaternion.y < 0.5f)
                hpFillImage.transform.rotation = Quaternion.Euler(Vector3.zero);
            else
                hpFillImage.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f));
        }

        //초기 HP 셋팅
        public void Initialize(int maxHealth, Transform target)
        {
            maxHp = maxHealth;
            hpFillImage.fillAmount = 1.0f;
            this.target = target;
        }

        //Damage 후 HP셋팅
        public void SetHP(int currentHp)
        {
            if (hpFillImage == null) return;

            hpFillImage.fillAmount = (float)currentHp / maxHp;
        }
    }
}

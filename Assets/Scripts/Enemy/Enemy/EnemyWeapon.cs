using Enemy_Player;
using UnityEngine;


namespace Enemy1
{
    public class EnemyWeapon : MonoBehaviour
    {
        [Header("Damage")]
        [SerializeField] private int damage = 1;
        [Header("공격 사정 거리")]
        [SerializeField] private float FindRange = 1.0f;
        [Header("Target Layer")]
        [SerializeField] private LayerMask targetLayer;
        private bool isAttack = false;
        public int Damage => damage;


        //공격 시작
        public void StartAttack()
        {
            isAttack = true;
        }
        //공격 종료
        public void EndAttack()
        {
            isAttack = false;
        }

        void Update()
        {
            if (!isAttack) return;
            //범위 안에 적 
            Collider2D EnemyObj = Physics2D.OverlapCircle(transform.position, FindRange, targetLayer);
            PlayerHealth enemyController = null;
            if (EnemyObj)
            {
                enemyController = EnemyObj.GetComponentInParent<PlayerHealth>();
            }
            // 적이라면 데미지 전달
            if (enemyController != null)
            {
                enemyController.TakeDamage(damage);
            }
            EndAttack();

        }
    }
}

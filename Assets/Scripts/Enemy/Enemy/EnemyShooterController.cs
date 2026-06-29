using UnityEngine;

namespace Enemy1
{
    public class EnemyShooterController : MonoBehaviour
    {
        [Header("발사위치")]
        [SerializeField] private Transform firePoint;
        [Header("총알 프리팹")]
        [SerializeField] private GameObject bulletPrefab;
        [Header("총알 딜레이")]
        [SerializeField] private float fireDelay = 0.2f;
        private float fireTimer = 0.0f;
        //탐지 반경
        [Header("원거리 공격 시작 거리")]
        [SerializeField] private float detectionRadius = 3.0f;
        // 적 레이어
        [Header("Target Layer")]
        [SerializeField] private LayerMask targetLayer;

        private void Update()
        {
            //delta 업데이트
            fireTimer += Time.deltaTime;
        }
        //play target 추척
        public void Fire()
        {

            if (fireTimer < fireDelay) return;
            fireTimer = 0.0f;

            // 사정거리 내의 적을 RaycastHit2D 배열로 탐지
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, detectionRadius, Vector2.zero, 0f, targetLayer);

            if (hits.Length > 0)
            {
                // 가장 가까운 적 방향으로 발사
                Transform target = GetNearestTarget(hits);
                Vector2 dir = (target.position - firePoint.position).normalized;
                Shoot(dir);
            }
        }
        void Shoot(Vector2 direction)
        {
            // 프리팹 생성
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // 총알 이동 스크립트에 방향 전달 (아래 2번 스크립트 참고)
            EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(direction);
            }
        }

        //가까운거리 
        Transform GetNearestTarget(RaycastHit2D[] hits)
        {
            Transform nearest = null;
            float minDistance = Mathf.Infinity;
            foreach (RaycastHit2D hit in hits)
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = hit.transform;
                }
            }
            return nearest;
        }
      
    }
}

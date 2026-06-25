using UnityEngine;

namespace Enemy1
{
    public class EnemyShooterController : MonoBehaviour
    {
        [Header("발사위치")]
        [SerializeField] private Transform firePoint;
        [Header("총알 프리팹")]
        [SerializeField] private GameObject bulletPrefab;

        [SerializeField] private float fireDelay = 0.2f;
        private float fireTimer = 0.0f;

        //발사 위치
        //Vector2 fireDirection = Vector2.down;

        //탐지 반경
        [SerializeField] private float detectionRadius = 3.0f;
        // 적 레이어
        [SerializeField] private LayerMask targetLayer;

        private void Update()
        {
            //delta 업데이트
            fireTimer += Time.deltaTime;
            SetTarget();
        }
        //play target 추척
        void SetTarget()
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

            //프리펩 Flip 설정
            if (direction.y <= firePoint.position.y)
            {
                if (direction.x <= firePoint.position.x)
                {
                    //왼쪽 아래
                    bulletScript.SetFlip(1, 1);
                }
                else
                {   //오른쪽 아래
                    bulletScript.SetFlip(0, 1);
                }
            }
            else
            {
                if (direction.x <= firePoint.position.x)
                {
                    //왼쪽 위
                    bulletScript.SetFlip(1, 0);
                }
                else
                {
                    //오른쪽 위
                    bulletScript.SetFlip(0, 0);
                }
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
       

        //public void Fire()
        //{
        //    //Bullet delta
        //    if (fireTimer < fireDelay) return;
        //    fireTimer = 0.0f;

        //    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        //    // 발사 방향 전달
        //    EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
        //    if (bulletScript != null)
        //    {
        //        bulletScript.SetDirection(fireDirection);


        //        //프리펩 Flip 설정
        //        if (fireDirection.y <= firePoint.position.y)
        //        {
        //            if (fireDirection.x <= firePoint.position.x)
        //            {
        //                //왼쪽 아래
        //                bulletScript.SetFlip(1, 1);
        //            }
        //            else
        //            {   //오른쪽 아래
        //                bulletScript.SetFlip(0, 1);
        //            }
        //        }
        //        else
        //        {
        //            if (fireDirection.x <= firePoint.position.x)
        //            {
        //                //왼쪽 위
        //                bulletScript.SetFlip(1, 0);
        //            }
        //            else
        //            {
        //                //오른쪽 위
        //                bulletScript.SetFlip(0, 0);
        //            }
        //        }
        //    }
        //}
        //Player 위치 전달
        //public void UpdateShooterState(Vector2 dir)
        //{
        //    fireDirection = dir;
        //}
    }
}

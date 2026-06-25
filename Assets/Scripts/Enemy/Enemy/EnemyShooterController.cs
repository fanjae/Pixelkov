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
        Vector2 fireDirection = Vector2.down;

        private void Update()
        
        {
            //delta 업데이트
            fireTimer += Time.deltaTime;
        }
        public void Fire()
        {
            //Bullet delta
            if (fireTimer < fireDelay) return;
            fireTimer = 0.0f;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // 발사 방향 전달
            EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(fireDirection);


                //프리펩 Flip 설정
                if (fireDirection.y <= firePoint.position.y)
                {
                    if (fireDirection.x <= firePoint.position.x)
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
                    if (fireDirection.x <= firePoint.position.x)
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
        }
        //Player 위치 전달
        public void UpdateShooterState(Vector2 dir)
        {
            fireDirection = dir;
        }
    }
}

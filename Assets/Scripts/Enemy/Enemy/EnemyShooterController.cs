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
            }
        }
        //Player 위치 전달
        public void UpdateShooterState(Vector2 dir)
        {
            fireDirection = dir;
        }
    }
}

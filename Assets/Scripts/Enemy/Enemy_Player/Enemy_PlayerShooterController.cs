using UnityEngine;

namespace Enemy_Player
{
    public class Enemy_PlayerShooterController : MonoBehaviour
    {
        [Header("발사위치")]
        [SerializeField] private Transform firePoint;
        [Header("총알 프리팹")]
        [SerializeField] private GameObject bulletPrefab;

        [SerializeField] private float fireDelay = 0.2f;
        private float fireTimer = 0.0f;

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
            Enemy_PlayerBullet bulletScript = bullet.GetComponent<Enemy_PlayerBullet>();
            if (bulletScript != null)
            {
                // 발사 방향
                bulletScript.SetDirection(fireDirection);
            }
        }

        // 발사  XY 좌표
        public void UpdateShooterState(float moveX, float moveY)
        {
            if (moveX != 0 || moveY != 0)
            {
                fireDirection.x = moveX;
                fireDirection.y = moveY;
            }
        }
    }
}

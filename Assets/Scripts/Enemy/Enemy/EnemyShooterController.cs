using UnityEngine;
using System.Collections;
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

        [Header("탄막 SKill")]
        [Header("탄막 Bullet")]
        [SerializeField] private GameObject bulletSkillPrefab; // 발사할 총알 프리팹
        [Header("탄막 스킬 총알 Count")]
        [SerializeField] private int bulletCount = 30;
        [Header("탄막 스킬 연속 Count")]
        [SerializeField] private int bulletSkillCount = 3;
        [Header("탄막 스킬 Delay")]
        [SerializeField] private float bulletSkillDelay = 0.5f;
        private bool isFireSkillStart = false;

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

     
        //탄막 스킬
        public void FireSkill()
        {
            if (isFireSkillStart) return;
            StartCoroutine(SpellStart());

        }
        IEnumerator SpellStart()
        {
            isFireSkillStart = true;
            float angle = 360 / bulletCount; 
            for(int i= 1; i<= bulletSkillCount; i++)
            {
                for (int k = 0; k < bulletCount; k++)
                {
                    GameObject obj = Instantiate(bulletSkillPrefab, firePoint.position, Quaternion.identity);
                    
                    obj.GetComponent<Rigidbody2D>().AddForce(
                        new Vector2(
                            Mathf.Cos(Mathf.PI * 2 * k / bulletCount),
                            Mathf.Sin(Mathf.PI * k * 2 / bulletCount)));
                    obj.transform.Rotate(new Vector3(0f, 0f, 360 * k / bulletCount - 90));
                }
                yield return new WaitForSeconds(bulletSkillDelay);
            }
            isFireSkillStart = false;

        } 
    }
}


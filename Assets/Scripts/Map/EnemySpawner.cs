using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // 스폰 적 목록
    [SerializeField] private GameObject[] enemyPrefabs;

    // 적 생성 후보 위치
    [SerializeField] private Transform[] spawnPoints;

    // 생성 간격
    [SerializeField] private float spawnInterval = 5f;

    // 최대 적 수
    [SerializeField] private int maxAliveCount = 5;

    // 살아있는 적 목록
    private readonly List<GameObject> aliveEnemies = new();

    private void Start()
    {
        // 게임 시작 시 반복 루틴
        StartCoroutine(SpawnRoutine());
    }

    // 적 생성 코루틴
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // 죽은 적 리스트 제거 
            aliveEnemies.RemoveAll(enemy => enemy == null);

            if (aliveEnemies.Count < maxAliveCount)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        // 프리팹 또는 스폰 위치 없으면 미생성
        if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // 선택된 위치에 적 생성
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 생성한 적을 생존 목록에 등록
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        aliveEnemies.Add(enemy);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject normalEnemyPrefab;
    public GameObject fastEnemyPrefab;
    public GameObject bossPrefab;

    [Header("Wave Settings")]
    private int currentWave = 1;
    public int enemiesInFirstWave = 5;
    private int enemiesToSpawn;
    private int enemiesLeftAlive;

    public float timeBetweenWaves = 4f;
    public float spawnDelay = 1f;

    [Header("Arena Boundaries")]
    public float minX = -14f; public float maxX = 14f;
    public float minZ = -14f; public float maxZ = 14f;

    private bool isWaveTransition = false;

    void Start()
    {
        StartNextWave();
    }

    void StartNextWave()
    {
        isWaveTransition = false;

        if (currentWave % 5 == 0)
        {
            enemiesToSpawn = 1;
            enemiesLeftAlive = 1;

            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateWaveUI(currentWave);
            }

            SpawnBoss();
        }
        else
        {
            enemiesToSpawn = enemiesInFirstWave + (currentWave - 1) * 3;
            enemiesLeftAlive = enemiesToSpawn;

            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateWaveUI(currentWave);
            }

            StartCoroutine(SpawnWaveRoutine());
        }
    }

    IEnumerator SpawnWaveRoutine()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            if (isWaveTransition) yield break;

            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnEnemy()
    {
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        Vector3 spawnPosition = new Vector3(randomX, 0.5f, randomZ);

        GameObject prefabToSpawn = normalEnemyPrefab;

        if (currentWave >= 2)
        {
            if (Random.value < 0.3f && fastEnemyPrefab != null)
            {
                prefabToSpawn = fastEnemyPrefab;
            }
        }

        GameObject enemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        enemy.GetComponent<Enemy>().OnEnemyDestroyed += EnemyKilled;
    }

    void SpawnBoss()
    {
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        Vector3 spawnPosition = new Vector3(randomX, 1f, randomZ);

        GameObject boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        boss.GetComponent<Enemy>().OnEnemyDestroyed += EnemyKilled;
    }

    void EnemyKilled()
    {
        enemiesLeftAlive--;

        if (enemiesLeftAlive <= 0 && !isWaveTransition)
        {
            isWaveTransition = true;

            if (currentWave == 5)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.GetComponent<PlayerShooting>().UnlockShotgun();
                }
            }

            currentWave++;

            StartCoroutine(WaitAndStartNextWave());
        }
    }

    IEnumerator WaitAndStartNextWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        StartNextWave();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject healthPrefab;
    public GameObject ammoPrefab;
    public GameObject barrelPrefab;
    public GameObject shieldPrefab;

    [Header("Spawn Intervals (Seconds)")]
    public float healthSpawnInterval = 20f;
    public float ammoSpawnInterval = 10f;
    public float barrelSpawnInterval = 30f;
    public float shieldSpawnInterval = 20f;

    [Header("Arena Boundaries")]
    public float minX = -12f;
    public float maxX = 12f;
    public float minZ = -12f;
    public float maxZ = 12f;
    public float spawnHeight = 0.5f;

    private float nextHealthSpawnTime;
    private float nextAmmoSpawnTime;
    private float nextBarrelSpawnTime;
    private float nextShieldSpawnTime;

    void Start()
    {
        nextHealthSpawnTime = Time.time + healthSpawnInterval;
        nextAmmoSpawnTime = Time.time + ammoSpawnInterval;
        nextBarrelSpawnTime = Time.time + barrelSpawnInterval;
        nextShieldSpawnTime = Time.time + shieldSpawnInterval;
    }

    void Update()
    {
        if (Time.time >= nextHealthSpawnTime)
        {
            SpawnResource(healthPrefab);
            nextHealthSpawnTime = Time.time + healthSpawnInterval;
        }

        if (Time.time >= nextAmmoSpawnTime)
        {
            SpawnResource(ammoPrefab);
            nextAmmoSpawnTime = Time.time + ammoSpawnInterval;
        }

        if (Time.time >= nextBarrelSpawnTime)
        {
            SpawnResource(barrelPrefab);
            nextBarrelSpawnTime = Time.time + barrelSpawnInterval;
        }
        if (Time.time >= nextShieldSpawnTime)
        {
            SpawnResource(shieldPrefab);
            nextShieldSpawnTime = Time.time + shieldSpawnInterval;
        }
    }

    void SpawnResource(GameObject prefab)
    {
        if (prefab == null) return;

        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        float currentHeight = (prefab == barrelPrefab) ? 0.1f : spawnHeight;

        Vector3 spawnPosition = new Vector3(randomX, currentHeight, randomZ);
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyWaveSpawner : MonoBehaviour {
    [SerializeField] private float baseEnemiesPerWave = 10f;
    [SerializeField] private float waveScalingFactor = 1.5f;
    [SerializeField] private float timeBetweenWaves = 3f;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private float minSpawnDistance = 15f;
    [SerializeField] private float maxSpawnDistance = 20f;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform playerTransform;
    private Camera mainCamera;
    private int currentWave = 0;
    private int enemiesRemainingInWave = 0;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool isSpawning = false;

    private void Start() {
        mainCamera = Camera.main;
        StartNextWave();
    }

    private void Update() {
        activeEnemies.RemoveAll(enemy => enemy == null);
        if (!isSpawning && activeEnemies.Count == 0) {
            StartCoroutine(StartNextWaveWithDelay());
        }
    }

    private IEnumerator StartNextWaveWithDelay() {
        yield return new WaitForSeconds(timeBetweenWaves);
        StartNextWave();
    }

    private void StartNextWave() {
        currentWave++;
        int enemiesThisWave = Mathf.RoundToInt(baseEnemiesPerWave * Mathf.Pow(waveScalingFactor, currentWave - 1));
        enemiesRemainingInWave = enemiesThisWave;
        StartCoroutine(SpawnWaveEnemies(enemiesThisWave));
    }

    private IEnumerator SpawnWaveEnemies(int enemyCount) {
        isSpawning = true;
        for (int i = 0; i < enemyCount; i++) {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
        isSpawning = false;
    }

    private void SpawnEnemy() {
        Vector2 spawnPosition = GetSpawnPositionOutsideCamera();
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        activeEnemies.Add(enemy);
    }

    private Vector2 GetSpawnPositionOutsideCamera() {
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        Vector2 offset = new Vector2(
            Mathf.Cos(angle) * spawnDistance,
            Mathf.Sin(angle) * spawnDistance
        );

        return (Vector2)playerTransform.position + offset;
    }

    public int GetCurrentWave() => currentWave;
    public int GetRemainingEnemies() => activeEnemies.Count;
    public float GetWaveProgress() => 1f - (float)activeEnemies.Count / enemiesRemainingInWave;
}
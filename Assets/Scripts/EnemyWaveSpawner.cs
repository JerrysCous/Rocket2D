using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyWaveSpawner : MonoBehaviour {
    [SerializeField] private float baseEnemiesPerWave = 10f; // Initial number of enemies
    [SerializeField] private float waveScalingFactor = 1.5f; // Multiplier for each new wave
    [SerializeField] private float spawnInterval = 0.2f; // Time between enemy spawns
    [SerializeField] private float minSpawnDistance = 15f; // Minimum distance from player to spawn
    [SerializeField] private float maxSpawnDistance = 20f; // Maximum distance from player to spawn
    [SerializeField] private GameObject[] enemyPrefabs; // Array of enemy types
    [SerializeField] private Transform playerTransform; // Reference to the player
    private Camera mainCamera;
    private int currentWave = 0; // Tracks the current wave
    public int enemiesRemainingInWave = 0; // Enemies left in the current wave
    private bool isSpawning = false; // Is the wave currently spawning
    public static EnemyWaveSpawner Instance; // Singleton reference

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        mainCamera = Camera.main;
        StartNextWave(); // Start the first wave
    }

    private void Update() {
        // Check if the current wave is completed
        if (!isSpawning && enemiesRemainingInWave <= 0) {
            StartNextWave(); // Start the next wave
        }
    }

    private void StartNextWave() {
        currentWave++;
        int enemiesThisWave = Mathf.RoundToInt(baseEnemiesPerWave * Mathf.Pow(waveScalingFactor, currentWave - 1));
        enemiesRemainingInWave = enemiesThisWave; // Update the count for this wave
        StartCoroutine(SpawnWaveEnemies(enemiesThisWave)); // Begin spawning
    }

    private IEnumerator SpawnWaveEnemies(int enemyCount) {
        isSpawning = true;
        for (int i = 0; i < enemyCount; i++) {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval); // Wait between spawns
        }
        isSpawning = false;
    }

    private void SpawnEnemy() {
        Vector2 spawnPosition = GetSpawnPositionOutsideCamera();
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]; // Pick a random enemy type
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
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
        return (Vector2)playerTransform.position + offset; // Spawn relative to the player's position
    }

    public void EnemyDefeated() {
        enemiesRemainingInWave--; // Decrement the remaining enemy count when an enemy is defeated
    }

    public int GetCurrentWave() => currentWave;
    public float GetWaveProgress() => 1f - (float)enemiesRemainingInWave / Mathf.RoundToInt(baseEnemiesPerWave * Mathf.Pow(waveScalingFactor, currentWave - 1));
}

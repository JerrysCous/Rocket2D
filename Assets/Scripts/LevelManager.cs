using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour {
    public static LevelManager Instance;
    public Tilemap tilemap;
    public List<CustomTile> tiles = new List<CustomTile>();
    public Transform player;
    public List<Transform> enemies = new List<Transform>();
    public Timer timer;
    public XPManager xpManager;
    public PlayerHealthBar playerHealthBar;

    private void Update() {
        //placeholder inputs for testing
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            SaveLevel();
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            LoadLevel();
        }
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

    public void RegisterEnemy(Transform enemy) {
        if (!enemies.Contains(enemy)) {
            enemies.Add(enemy);
        }
    }

    public void UnregisterEnemy(Transform enemy) {
        if (enemies.Contains(enemy)) {
            enemies.Remove(enemy);
        }
    }

    void LoadLevel() {
        string json = File.ReadAllText(Application.dataPath + "/gameSave.json");
        LevelData data = JsonUtility.FromJson<LevelData>(json);

        // load timer and xp
        if (timer != null) {
            timer.SetTime(data.currentTime);
        }

        if (xpManager != null) {
            xpManager.SetXP(data.currentXP, data.playerLevel);
        }

        if (playerHealthBar != null) {
            playerHealthBar.SetHealth(data.currentHealth);
        }

        // load tiles
        tilemap.ClearAllTiles();
        for (int i = 0; i < data.tiles.Count; i++) {
            tilemap.SetTile(
                new Vector3Int(data.poses_x[i], data.poses_y[i], 0),
                tiles.Find(t => t.name == data.tiles[i]).tile
            );
        }

        if (player != null) {
            player.position = new Vector3(data.playerPosition.x, data.playerPosition.y, data.playerPosition.z);
            player.rotation = Quaternion.Euler(data.playerRotation.x, data.playerRotation.y, data.playerRotation.z);
        }

        // load enemies
        for (int i = 0; i < data.enemiesPositions.Count; i++) {
            if (i < enemies.Count) {
                enemies[i].position = new Vector3(
                    data.enemiesPositions[i].x,
                    data.enemiesPositions[i].y,
                    data.enemiesPositions[i].z
                );
                enemies[i].rotation = Quaternion.Euler(
                    data.enemiesRotations[i].x,
                    data.enemiesRotations[i].y,
                    data.enemiesRotations[i].z
                );
            }
        }
    }

    void SaveLevel() {
        BoundsInt bounds = tilemap.cellBounds;
        LevelData levelData = new LevelData();

        LevelData data = new LevelData();

        // save timer and xp
        if (timer != null) {
            levelData.currentTime = timer.GetCurrentTime();
        }

        if (xpManager != null) {
            levelData.currentXP = xpManager.GetCurrentXP();
            levelData.playerLevel = xpManager.GetCurrentLevel();
        }

        // save health
        if (playerHealthBar != null) {
            levelData.currentHealth = playerHealthBar.GetCurrentHealth();
        }

        // save tiles
        for (int x = bounds.min.x; x < bounds.max.x; x++) {
            for (int y = bounds.min.y; y < bounds.max.y; y++) {
                TileBase temp = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (temp != null) {
                    CustomTile tempTile = tiles.Find(t => t.tile == temp);
                    if (tempTile != null) {
                        levelData.tiles.Add(tempTile.id);
                        levelData.poses_x.Add(x);
                        levelData.poses_y.Add(y);
                    }
                    else {
                        Debug.LogWarning($"tile not found in the tiles list: {temp}");
                    }
                }
            }
        }

        // save player position
        if (player != null) {
            levelData.playerPosition = new Vector3Data(player.position);
            levelData.playerRotation = new Vector3Data(player.rotation.eulerAngles);
        }

        // save enemies
        foreach (var enemy in enemies) {
            levelData.enemiesPositions.Add(new Vector3Data(enemy.position));
            levelData.enemiesRotations.Add(new Vector3Data(enemy.rotation.eulerAngles));
        }

        string json = JsonUtility.ToJson(levelData, true);
        File.WriteAllText(Application.dataPath + "/saveGame.json", json);
    }



}

[System.Serializable]
public class LevelData {
    public List<string> tiles = new List<string>();
    public List<int> poses_x = new List<int>();
    public List<int> poses_y = new List<int>();

    public Vector3Data playerPosition;
    public Vector3Data playerRotation;

    public float currentTime;
    public float currentXP;
    public int playerLevel;

    public float currentHealth;

    public List<Vector3Data> enemiesPositions = new List<Vector3Data>();
    public List<Vector3Data> enemiesRotations = new List<Vector3Data>();
}

[System.Serializable]
public class Vector3Data {
    public float x, y, z;

    public Vector3Data(Vector3 vector) {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }
}
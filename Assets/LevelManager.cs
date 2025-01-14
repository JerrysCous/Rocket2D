using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public Tilemap tilemap;

    private void Update() {
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

    void LoadLevel() {
        string json = File.ReadAllText(Application.dataPath + "/testLevel.json");
        LevelData data = JsonUtility.FromJson<LevelData>(json);

        tilemap.ClearAllTiles();

        for (int i = 0; i < data.poses.Count; i++) {
            tilemap.SetTile(data.poses[i], data.tiles[i]);
        }
    }

    void SaveLevel() {
        BoundsInt bounds = tilemap.cellBounds;
        LevelData levelData = new LevelData();

        for (int x = bounds.min.x; x < bounds.max.x; x++) {
            for (int y = bounds.min.y; y < bounds.max.y; y++) {
                TileBase temp = tilemap.GetTile(new Vector3Int(x, y, 0));

                if (temp != null) {
                    levelData.tiles.Add(temp);
                    levelData.poses.Add(new Vector3Int(x, y, 0));
                }
            }
        }

        string json = JsonUtility.ToJson(levelData, true);
        File.WriteAllText(Application.dataPath + "/testLevel.json", json);

    }
}

public class LevelData {
    public List<string> tiles = new List<string>();
    public List<Vector3Int> poses = new List<Vector3Int>();
}

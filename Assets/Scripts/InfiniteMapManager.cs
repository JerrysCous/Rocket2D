using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InfiniteMapManager : MonoBehaviour {
    public Tilemap tilemap;
    public Tile[] floorTiles;
    public Tile[] bigTiles;
    public Tile[] craterTiles;
    public Tile treeTile;
    public Tile pondTile;
    public int chunkSize = 16;
    public int viewDistance = 2;
    public float treeChance = 0.1f;
    public float pondChance = 0.3f;
    public float bigTileChance = 0.05f;
    public float craterChance = 0.1f;
    public float updateInterval = 0.5f;
    private Camera mainCamera;
    private Vector2Int currentChunk;
    private HashSet<Vector2Int> loadedChunks;
    private System.Random seededRandom;
    private float nextUpdateTime;
    private bool initialGeneration = true;

    private void Start() {
        mainCamera = Camera.main;
        loadedChunks = new HashSet<Vector2Int>();
        seededRandom = new System.Random(Environment.TickCount);
        if (tilemap == null) tilemap = GetComponent<Tilemap>();
        ValidateComponents();
        ForceMapGeneration();
    }

    private void Update() {
        if (Time.time >= nextUpdateTime || initialGeneration) {
            UpdateChunks();
            nextUpdateTime = Time.time + updateInterval;
            initialGeneration = false;
        }
    }

    private void ForceMapGeneration() {
        Vector3 playerPos = mainCamera.transform.position;
        currentChunk = WorldToChunkCoordinates(playerPos);
        UpdateChunks();
    }

    private void UpdateChunks() {
        Vector3 playerPos = mainCamera.transform.position;
        Vector2Int newChunk = WorldToChunkCoordinates(playerPos);
        if (newChunk == currentChunk && !initialGeneration) return;
        currentChunk = newChunk;
        HashSet<Vector2Int> chunksToKeep = new HashSet<Vector2Int>();
        for (int x = -viewDistance; x <= viewDistance; x++) {
            for (int y = -viewDistance; y <= viewDistance; y++) {
                Vector2Int chunkCoord = currentChunk + new Vector2Int(x, y);
                chunksToKeep.Add(chunkCoord);

                if (!loadedChunks.Contains(chunkCoord)) {
                    GenerateChunk(chunkCoord);
                    loadedChunks.Add(chunkCoord);
                }
            }
        }

        List<Vector2Int> chunksToRemove = new List<Vector2Int>();
        foreach (Vector2Int chunk in loadedChunks) {
            if (!chunksToKeep.Contains(chunk)) {
                chunksToRemove.Add(chunk);
                RemoveChunk(chunk);
            }
        }

        foreach (Vector2Int chunk in chunksToRemove) {
            loadedChunks.Remove(chunk);
        }
    }

    private Vector2Int WorldToChunkCoordinates(Vector3 worldPosition) {
        return new Vector2Int(
            Mathf.FloorToInt(worldPosition.x / chunkSize),
            Mathf.FloorToInt(worldPosition.y / chunkSize)
        );
    }

    private void GenerateChunk(Vector2Int chunkCoord) {
        System.Random chunkRandom = new System.Random(
            HashCode.Combine(chunkCoord.x, chunkCoord.y, seededRandom.Next())
        );

        Vector3Int chunkOffset = new Vector3Int(
            chunkCoord.x * chunkSize,
            chunkCoord.y * chunkSize,
            0
        );

        for (int x = 0; x < chunkSize; x++) {
            for (int y = 0; y < chunkSize; y++) {
                Vector3Int tilePosition = chunkOffset + new Vector3Int(x, y, 0);
                GenerateTile(tilePosition, chunkRandom);
            }
        }
    }

    private void GenerateTile(Vector3Int position, System.Random random) {
        tilemap.SetTile(position, floorTiles[random.Next(floorTiles.Length)]);
        double roll = random.NextDouble();
        if (roll < bigTileChance && position.x % 2 == 0 && position.y % 2 == 0) {
            // Place big tile if there's space
            if (IsTileAreaClear(position, 2, 2)) {
                PlaceBigTile(position, random);
                return;
            }
        }

        roll = random.NextDouble();
        if (roll < craterChance) {
            tilemap.SetTile(position, craterTiles[random.Next(craterTiles.Length)]);
            return;
        }

        roll = random.NextDouble();
        if (roll < treeChance) {
            tilemap.SetTile(position, treeTile);

            // Try to place pond near tree
            if (random.NextDouble() < pondChance) {
                Vector3Int pondPos = position + new Vector3Int(
                    random.Next(-1, 2),
                    random.Next(-1, 2),
                    0
                );
                if (tilemap.GetTile(pondPos) == floorTiles[0] || tilemap.GetTile(pondPos) == floorTiles[1]) {
                    tilemap.SetTile(pondPos, pondTile);
                }
            }
        }
    }

    private bool IsTileAreaClear(Vector3Int position, int width, int height) {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Vector3Int checkPos = position + new Vector3Int(x, y, 0);
                if (tilemap.GetTile(checkPos) != floorTiles[0] &&
                    tilemap.GetTile(checkPos) != floorTiles[1]) {
                    return false;
                }
            }
        }
        return true;
    }

    private void PlaceBigTile(Vector3Int position, System.Random random) {
        tilemap.SetTile(position, bigTiles[0]);
        tilemap.SetTile(position + new Vector3Int(1, 0, 0), bigTiles[1]);
    }

    private void RemoveChunk(Vector2Int chunkCoord) {
        Vector3Int chunkOffset = new Vector3Int(
            chunkCoord.x * chunkSize,
            chunkCoord.y * chunkSize,
            0
        );

        for (int x = 0; x < chunkSize; x++) {
            for (int y = 0; y < chunkSize; y++) {
                tilemap.SetTile(chunkOffset + new Vector3Int(x, y, 0), null);
            }
        }
    }

    private void ValidateComponents() {
        if (floorTiles == null || floorTiles.Length == 0)
            Debug.LogError("Floor tiles array is empty!");
        if (bigTiles == null || bigTiles.Length == 0)
            Debug.LogError("Big tiles array is empty!");
        if (craterTiles == null || craterTiles.Length == 0)
            Debug.LogError("Crater tiles array is empty!");
        if (treeTile == null)
            Debug.LogError("Tree tile is not assigned!");
        if (pondTile == null)
            Debug.LogError("Pond tile is not assigned!");
    }
}
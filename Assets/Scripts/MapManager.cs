using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public Tilemap tilemap;          // Reference to the Tilemap component
    public Tile[] floorTiles;        // Array of floor tiles (FloorTile, FloorTile2)
    public Tile[] bigTiles;          // Array of big tiles (TileBig1, TileBig2)
    public Tile[] craterTiles;       // Array of crater tiles (TileCrator1, TileCrator2)
    public Tile treeTile;            // Tree tile (TileTree)
    public Tile pondTile;            // Pond tile (TilePond)
    public int chunkSize = 10;       // Size of each chunk (each tile can represent 1 chunk)
    public float treeChance = 0.1f;  // Chance to spawn a tree (10% for example)
    public float pondChance = 0.3f;  // Chance to spawn a pond near a tree (30% chance)
    public float bigTileChance = 0.05f; // Chance to spawn big tiles (5% chance)
    public float craterChance = 0.1f; // Chance to spawn a crater (10% chance)

    private Camera mainCamera;       // Camera to get the screen bounds
    private Vector3 lastPlayerPosition; // Track the last player's position

    void Start()
    {
        mainCamera = Camera.main;
        lastPlayerPosition = transform.position; // Assume the MapManager is tied to the player or main camera

        if (tilemap == null) tilemap = GetComponent<Tilemap>();

        if (floorTiles.Length == 0) UnityEngine.Debug.LogError("Floor tiles array is empty!");

        if (treeTile == null) UnityEngine.Debug.LogError("Tree tile is not assigned!");

        // Generate the map at the start to fill the screen
        FillScreenWithMap();
    }

    void Update()
    {
        // Update the map generation based on the player's movement
        if (Vector3.Distance(transform.position, lastPlayerPosition) >= chunkSize)
        {
            lastPlayerPosition = transform.position;
            GenerateMap(); // Continually spawn new chunks as the player moves
        }
    }

    void FillScreenWithMap()
    {
        // Get camera width and height in world space
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Calculate how many tiles fit horizontally and vertically
        int tilesX = Mathf.CeilToInt(cameraWidth / chunkSize);
        int tilesY = Mathf.CeilToInt(cameraHeight / chunkSize);

        // Debugging the tile map size
        UnityEngine.Debug.Log($"Tiles to generate: {tilesX} x {tilesY}");

        // Generate map to fill the screen based on calculated tile dimensions
        GenerateMap(tilesX, tilesY);
    }

    void GenerateMap(int mapWidth = 20, int mapHeight = 20)
    {
        // Loop through all positions of the tilemap and spawn tiles
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                // Randomly place FloorTile or FloorTile2
                if (UnityEngine.Random.value < 0.5f)
                {
                    tilemap.SetTile(tilePosition, floorTiles[0]);
                }
                else
                {
                    tilemap.SetTile(tilePosition, floorTiles[1]);
                }

                // Randomly place Trees with a set chance
                if (UnityEngine.Random.value < treeChance)
                {
                    tilemap.SetTile(tilePosition, treeTile);

                    // Try to place a pond near the tree
                    if (UnityEngine.Random.value < pondChance)
                    {
                        Vector3Int pondPosition = tilePosition + new Vector3Int(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2), 0);
                        tilemap.SetTile(pondPosition, pondTile);
                    }
                }

                // Randomly place Big Tiles
                if (UnityEngine.Random.value < bigTileChance)
                {
                    tilemap.SetTile(tilePosition, bigTiles[0]);
                    if (bigTiles.Length > 1)
                    {
                        Vector3Int adjacentPosition = tilePosition + new Vector3Int(1, 0, 0);
                        tilemap.SetTile(adjacentPosition, bigTiles[1]);
                    }
                }

                // Randomly place Craters
                if (UnityEngine.Random.value < craterChance)
                {
                    if (UnityEngine.Random.value < 0.5f)
                    {
                        tilemap.SetTile(tilePosition, craterTiles[0]);
                    }
                    else
                    {
                        tilemap.SetTile(tilePosition, craterTiles[1]);
                    }
                }
            }
        }
    }

    // Add your endless generation logic here (spawn new tiles when needed, remove old ones)
}


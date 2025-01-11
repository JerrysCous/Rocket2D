using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public Tilemap tilemap;  // Reference to the Tilemap component
    public Tile[] floorTiles; // Array of floor tiles (FloorTile, FloorTile2)
    public Tile[] bigTiles;   // Array of big tiles (TileBig1, TileBig2)
    public Tile[] craterTiles; // Array of crater tiles (TileCrator1, TileCrator2)
    public Tile treeTile;     // Tree tile (TileTree)
    public Tile pondTile;     // Pond tile (TilePond)

    public int mapWidth = 20;  // Width of the map
    public int mapHeight = 20; // Height of the map
    public float treeChance = 0.1f; // Chance to spawn a tree (10% for example)
    public float pondChance = 0.3f; // Chance to spawn a pond near a tree (30% chance)
    public float bigTileChance = 0.05f; // Chance to spawn big tiles (5% chance)
    public float craterChance = 0.1f; // Chance to spawn a crater (10% chance)

    void Start()
    {
        // Check if essential tiles are assigned before generating the map
        if (floorTiles.Length == 0)
        {
           UnityEngine.Debug.LogError("Floor tiles array is empty!");
        }
        if (treeTile == null)
        {
           UnityEngine.Debug.LogError("Tree tile is not assigned!");
        }

        // Call the method to generate the map
        GenerateMap();
    }

    void GenerateMap()
    {
        // Loop through all the positions of the tilemap
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                // Randomly place FloorTile or FloorTile2
                if (UnityEngine.Random.value < 0.5f)
                {
                    if (floorTiles.Length > 0)  // Check if the array is not empty
                    {
                        tilemap.SetTile(tilePosition, floorTiles[0]); // FloorTile
                    }
                }
                else
                {
                    if (floorTiles.Length > 1)  // Check if the array has a second element
                    {
                        tilemap.SetTile(tilePosition, floorTiles[1]); // FloorTile2
                    }
                }

                // Randomly place Trees with a set chance
                if (UnityEngine.Random.value < treeChance)
                {
                    if (treeTile != null)  // Ensure treeTile is assigned
                    {
                        tilemap.SetTile(tilePosition, treeTile);

                        // If a tree is placed, try to place a pond nearby
                        if (UnityEngine.Random.value < pondChance)
                        {
                            Vector3Int pondPosition = tilePosition + new Vector3Int(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2), 0);
                            if (pondTile != null)  // Ensure pondTile is assigned
                            {
                                tilemap.SetTile(pondPosition, pondTile);
                            }
                        }
                    }
                }

                // Randomly place the Big Crater tiles
                if (UnityEngine.Random.value < bigTileChance)
                {
                    if (bigTiles.Length > 0)  // Check if bigTiles array is not empty
                    {
                        Vector3Int bigTilePosition = tilePosition;
                        tilemap.SetTile(bigTilePosition, bigTiles[0]); // TileBig1

                        if (bigTiles.Length > 1)  // Ensure there are two elements for bigTiles
                        {
                            Vector3Int adjacentPosition = tilePosition + new Vector3Int(1, 0, 0); // Place TileBig2 to the right
                            tilemap.SetTile(adjacentPosition, bigTiles[1]); // TileBig2
                        }
                    }
                }

                // Randomly place craters (TileCrator1, TileCrator2)
                if (UnityEngine.Random.value < craterChance)
                {
                    if (craterTiles.Length > 0)  // Check if craterTiles array is not empty
                    {
                        if (UnityEngine.Random.value < 0.5f)
                        {
                            tilemap.SetTile(tilePosition, craterTiles[0]); // TileCrator1
                        }
                        else
                        {
                            if (craterTiles.Length > 1)  // Ensure there are at least two elements in craterTiles
                            {
                                tilemap.SetTile(tilePosition, craterTiles[1]); // TileCrator2
                            }
                        }
                    }
                }
            }
        }
    }
}


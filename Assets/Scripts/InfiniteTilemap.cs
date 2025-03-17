using System.Collections.Generic;
using UnityEngine;

public class InfiniteTilemap : MonoBehaviour
{
    public GameObject[] tilemapPrefabs; // Assign predefined tilemaps
    public Transform player;
    public GameObject grid;

    private Vector2 currentTile;
    private Dictionary<Vector2, GameObject> activeTiles = new Dictionary<Vector2, GameObject>();
    private Queue<GameObject> tilePool = new Queue<GameObject>();

    private float tileSize = 44f; // Size of each tilemap
    private int gridSize = 1; // 3x3 area around the player

    void Start()
    {
        currentTile = GetTilePosition(player.position);
        GenerateInitialTiles();
    }

    void Update()
    {
        Vector2 playerTile = GetTilePosition(player.position);

        if (playerTile != currentTile)
        {
            currentTile = playerTile;
            UpdateTiles();
        }
    }

    Vector2 GetTilePosition(Vector3 position)
    {
        return new Vector2(
            Mathf.Floor(position.x / tileSize),
            Mathf.Floor(position.y / tileSize)
        );
    }

    void GenerateInitialTiles()
    {
        for (int x = -gridSize; x <= gridSize; x++)
        {
            for (int y = -gridSize; y <= gridSize; y++)
            {
                Vector2 tilePos = currentTile + new Vector2(x, y);
                SpawnTile(tilePos);
            }
        }
    }

    void UpdateTiles()
    {
        HashSet<Vector2> neededTiles = new HashSet<Vector2>();

        for (int x = -gridSize; x <= gridSize; x++)
        {
            for (int y = -gridSize; y <= gridSize; y++)
            {
                Vector2 newTilePos = currentTile + new Vector2(x, y);
                neededTiles.Add(newTilePos);

                if (!activeTiles.ContainsKey(newTilePos))
                {
                    SpawnTile(newTilePos);
                }
            }
        }

        CleanupTiles(neededTiles);
    }

    // void SpawnTile(Vector2 position)
    // {
    //     GameObject tile;
    //     if (tilePool.Count > 0)
    //     {
    //         tile = tilePool.Dequeue();
    //         tile.SetActive(true);
    //     }
    //     else
    //     {
    //         tile = Instantiate(tilemapPrefabs[Random.Range(0, tilemapPrefabs.Length)]);
    //         tile.transform.parent = grid.transform;
    //     }

    //     tile.transform.position = position * tileSize;
    //     tile.transform.localScale = Vector3.one;
    //     activeTiles[position] = tile;
    // }

    void SpawnTile(Vector2 position)
    {
        GameObject tile;
        if (tilePool.Count > 0)
        {
            tile = tilePool.Dequeue();
            tile.SetActive(true);
        }
        else
        {
            int selectedIndex = GetUniqueTileIndex(position);
            tile = Instantiate(tilemapPrefabs[selectedIndex]);
            tile.transform.parent = grid.transform;
        }

        tile.transform.position = position * tileSize;
        tile.transform.localScale = Vector3.one;
        activeTiles[position] = tile;
    }

    int GetUniqueTileIndex(Vector2 position)
    {
        HashSet<int> neighborTypes = new HashSet<int>();

        // Check adjacent tiles
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        foreach (Vector2 dir in directions)
        {
            Vector2 neighborPos = position + dir;
            if (activeTiles.ContainsKey(neighborPos))
            {
                GameObject neighbor = activeTiles[neighborPos];
                int neighborIndex = System.Array.IndexOf(tilemapPrefabs, neighbor);
                if (neighborIndex != -1) neighborTypes.Add(neighborIndex);
            }
        }

        // Choose a tile that is not in neighborTypes
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < tilemapPrefabs.Length; i++)
        {
            if (!neighborTypes.Contains(i))
            {
                availableIndices.Add(i);
            }
        }

        return availableIndices.Count > 0 ? availableIndices[Random.Range(0, availableIndices.Count)] : Random.Range(0, tilemapPrefabs.Length);
    }


    void CleanupTiles(HashSet<Vector2> neededTiles)
    {
        List<Vector2> toRemove = new List<Vector2>();

        foreach (var tile in activeTiles)
        {
            if (!neededTiles.Contains(tile.Key))
            {
                toRemove.Add(tile.Key);
                tile.Value.SetActive(false);
                tilePool.Enqueue(tile.Value);
            }
        }

        foreach (var pos in toRemove)
        {
            activeTiles.Remove(pos);
        }
    }
}

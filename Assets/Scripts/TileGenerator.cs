using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour {

    [SerializeField] Tile[] tiles;
    [SerializeField] int rows = 1;
    [SerializeField] int columns = 1;
    [SerializeField] Vector3 midPoint;
    private Tile lastTile;
    [SerializeField] GameObject wallElement;

	// Use this for initialization
	void Start () {
        GenerateMap(midPoint);
        //CreateGameObjectFromTile(GetTile(), wallHeightBase);
	}

    private void GenerateMap(Vector3 midPoint)
    {
        GameObject labyrinthParent = new GameObject("Labyrinth");
        int midNumberX = rows / 2;
        int midNumberY = columns / 2;
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                if (x != midNumberX || y != midNumberY)
                {
                    GameObject newObj = CreateGameObjectFromTile(GetTile());
                    newObj.transform.parent = labyrinthParent.transform;
                    newObj.transform.position = new Vector3(x * lastTile.extentSize + (midPoint.x - ((rows * 0.5f) * lastTile.extentSize)),
                    midPoint.y,
                    y * lastTile.extentSize + (midPoint.z - ((rows * 0.5f) * lastTile.extentSize)));
                }
            }
        }

        SetWalls(midPoint, midNumberX, midNumberY, labyrinthParent.transform);
    }

    private void SetWalls(Vector3 midPoint, int midNumberX, int midNumberY, Transform parent)
    {
        int doorPoint;
        bool doorInSideWalls = Random.value > 0.5f? true : false;
        bool doorInFirst = Random.value > 0.5f ? true : false;
        if(doorInSideWalls)
        {
            doorPoint = Random.Range(-midNumberY * (int)lastTile.extentSize, columns * (int)lastTile.extentSize - (midNumberY * (int)lastTile.extentSize));
        }
        else
        {
            doorPoint = Random.Range(-midNumberX * (int)lastTile.extentSize, rows * (int)lastTile.extentSize - (midNumberX * (int)lastTile.extentSize));
        }
        for (int y = -midNumberY * (int)lastTile.extentSize; y < columns * lastTile.extentSize - (midNumberY * (int)lastTile.extentSize); y++)
        {
            if (!doorInSideWalls || !doorInFirst || y != doorPoint && y - 1 != doorPoint && y + 1 != doorPoint && doorInSideWalls)
            {
                // Left Wall
                Instantiate(wallElement, new Vector3((midPoint.x - (midNumberX * lastTile.extentSize) - lastTile.extentSize),
                    midPoint.y,
                    (y + midPoint.z) - lastTile.extentSize), Quaternion.identity, parent);
            }
            if (!doorInSideWalls || doorInFirst || y != doorPoint && y - 1 != doorPoint && y + 1 != doorPoint && doorInSideWalls)
            {
                // Right Wall
                Instantiate(wallElement, new Vector3(midPoint.x + (midNumberX * lastTile.extentSize),
                    midPoint.y,
                    y + midPoint.z - lastTile.extentSize), Quaternion.identity, parent);
            }
        }
        for (int x = -midNumberX * (int)lastTile.extentSize; x < rows * lastTile.extentSize - (midNumberX * (int)lastTile.extentSize); x++)
        {
            if (doorInSideWalls || !doorInFirst || x != doorPoint && x - 1 != doorPoint && x + 1 != doorPoint && !doorInSideWalls)
            {
                // Bottom Wall
                Instantiate(wallElement, new Vector3(x + midPoint.x - lastTile.extentSize,
                    midPoint.y,
                    (midPoint.z - midNumberY * lastTile.extentSize) - lastTile.extentSize), Quaternion.Euler(new Vector3(0f, 90f, 0f)), parent);
            }
            if (doorInSideWalls || doorInFirst || x != doorPoint && x - 1 != doorPoint && x + 1 != doorPoint && !doorInSideWalls)
            { 
                // Top Wall
                Instantiate(wallElement, new Vector3(x + midPoint.x - lastTile.extentSize,
                    midPoint.y,
                    midPoint.z + midNumberY * lastTile.extentSize), Quaternion.Euler(new Vector3(0f, 90f, 0f)), parent);
            }
        }
        // Set wall in the upper right corner
        Instantiate(wallElement, new Vector3(midPoint.x + midNumberX * lastTile.extentSize, midPoint.y, midPoint.z + midNumberY * lastTile.extentSize), Quaternion.identity, parent);
    }

    // Here are the rules for the generation supposed to go
    private Tile GetTile()
    {
        if(lastTile)
        {
            
            if (lastTile.directions.Contains(Helper.Directions.Left))
            {
                GetRandomTileForExit(Helper.Directions.Right);
            }
            else if (lastTile.directions.Contains(Helper.Directions.Right))
            {
                GetRandomTileForExit(Helper.Directions.Left);
            }
        }
        return tiles[Random.Range(0, tiles.Length)];
    }

    private Tile GetRandomTileForExit(Helper.Directions desiredDirection)
    {
        List<Tile> desiredTiles = new List<Tile>();
        foreach (Tile tile in tiles)
        {
            if (tile.directions.Contains(desiredDirection))
            {
                desiredTiles.Add(tile);
            }
        }
        return desiredTiles[Random.Range(0, desiredTiles.Count)];
    }

    private GameObject CreateGameObjectFromTile(Tile tile)
    {
        lastTile = tile;
        GameObject newTile = new GameObject();
        if(tile.directions.Contains(Helper.Directions.Bottom))
        {
            Instantiate(Random.value > 0.2f? tile.way : tile.torchWall, new Vector3(0f, 0f, -((tile.extentSize / 5) + (tile.extentSize / 10))), Quaternion.Euler(new Vector3(0f, 0f, 0f)), newTile.transform);
        }
        if (tile.directions.Contains(Helper.Directions.Top))
        {
            Instantiate(Random.value > 0.2f ? tile.way : tile.torchWall, new Vector3(0f, 0f, ((tile.extentSize / 5) + (tile.extentSize / 10))), Quaternion.Euler(new Vector3(0f, 0f, 0f)), newTile.transform);
        }
        if (tile.directions.Contains(Helper.Directions.Left))
        {
            Instantiate(Random.value > 0.2f ? tile.way : tile.torchWall, new Vector3(-((tile.extentSize / 5) + (tile.extentSize / 10)), 0f, 0f), Quaternion.Euler(new Vector3(0f, -90f, 0f)), newTile.transform);
        }
        if (tile.directions.Contains(Helper.Directions.Right))
        {
            Instantiate(Random.value > 0.2f ? tile.way : tile.torchWall
                , new Vector3(((tile.extentSize / 5) + (tile.extentSize / 10)), 0f, 0f), Quaternion.Euler(new Vector3(0f, 90f, 0f)), newTile.transform);
        }
        Instantiate(tile.middle, new Vector3(0f, 0f, 0f), Quaternion.Euler(Vector3.zero), newTile.transform);
        return newTile;
    }
}

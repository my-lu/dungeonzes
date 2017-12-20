using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    const int hideoutColumns = 5;
    const int hideoutRows = 3;

    int diff = 0;
    int columns = 8;
    int rows = 8;
    float enemyFloat = 2.0f;
    Count wallCount = new Count(5, 9);
    Count itemCount = new Count(1, 3);

    public GameObject exit;
    public GameObject hideoutExit;
    public GameObject[] floorTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] wallTiles;
    public GameObject[] enemyTiles;
    public GameObject[] itemTiles;
    public GameObject stash;

    Transform boardHolder;
    List<Vector3> gridPositions = new List<Vector3>();

    public void UpdateLevel(Level level)
    {
        diff = level.difficulty;
        rows = level.boardSize[0];
        columns = level.boardSize[1];
        enemyFloat = level.enemyQuantity;
        wallCount = new Count(level.numWalls[0], level.numWalls[1]);
        itemCount = new Count(1, level.itemQuantity);
    }

    void InitializeList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0.0f));
            }
        }
    }

    void LevelBoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0.0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    void HideoutBoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        for (int x = -1; x < hideoutColumns + 1; x++)
        {
            for (int y = -1; y < hideoutRows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == hideoutColumns || y == -1 || y == hideoutRows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0.0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);
        for(int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    void LayoutHideoutObjects()
    {
        Instantiate(stash, new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity);
    }

    public void SetupScene(int level)
    {
        LevelBoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(itemTiles, itemCount.minimum, itemCount.maximum);
        int enemyCount = (int)Mathf.Log(level, enemyFloat) + diff;
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0.0f), Quaternion.identity);
    }

    public void SetupHideout()
    {
        HideoutBoardSetup();
        LayoutHideoutObjects();
        Instantiate(hideoutExit, new Vector3(hideoutColumns - 1, hideoutRows - 1, 0.0f), Quaternion.identity);
    }
}

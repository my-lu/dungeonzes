using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int difficulty;
    public int[] boardSize;
    public int[] numWalls;
    public int floorQuantity;
    public int itemQuantity;
    public float enemyQuantity;

    const int boardSmallMin = 8;
    const int boardSmallMax = 10;
    const int boardNormalMin = 12;
    const int boardNormalMax = 15;
    const int boardLargeMin = 20;
    const int boardLargeMax = 25;

    const int floorsSmallMin = 3;
    const int floorsSmallMax = 4;
    const int floorsNormalMin = 5;
    const int floorsNormalMax = 6;
    const int floorsLargeMin = 7;
    const int floorsLargeMax = 8;

    const int itemEasy = 3;
    const int itemNormal = 2;
    const int itemHard = 1;

    const float enemyEasy = 3.0f;
    const float enemyNormal = 2.0f;
    const float enemyHard = 1.5f;

    public Level(int diff)
    {
        difficulty = diff;
        int counter = Probabilities(diff);
        if (counter == 0)
        {
            boardSize = new int[2] { Random.Range(boardSmallMin, boardSmallMax + 1), Random.Range(boardSmallMin, boardSmallMax + 1) };
        }
        else if (counter == 1)
        {
            boardSize = new int[2] { Random.Range(boardNormalMin, boardNormalMax + 1), Random.Range(boardNormalMin, boardNormalMax + 1) };
        }
        else
        {
            boardSize = new int[2] { Random.Range(boardLargeMin, boardLargeMax + 1), Random.Range(boardLargeMin, boardLargeMax + 1) };
        }
        numWalls = new int[2] { Mathf.Max(boardSize[0], boardSize[1]), 2 * Mathf.Min(boardSize[0], boardSize[1]) };

        counter = Probabilities(diff);
        if (counter == 0)
            floorQuantity = Random.Range(floorsSmallMin, floorsSmallMax + 1);
        else if (counter == 1)
            floorQuantity = Random.Range(floorsNormalMin, floorsNormalMax + 1);
        else
            floorQuantity = Random.Range(floorsLargeMin, floorsLargeMax + 1);

        counter = Probabilities(diff);
        if (counter == 0)
            itemQuantity = itemEasy;
        else if (counter == 1)
            itemQuantity = itemNormal;
        else
            itemQuantity = itemHard;

        counter = Probabilities(diff);
        if (counter == 0)
            enemyQuantity = enemyEasy;
        else if (counter == 1)
            enemyQuantity = enemyNormal;
        else
            enemyQuantity = enemyHard;
    }

    int Probabilities(int diff)
    {
        if (diff < 0 || diff > 2)
            diff = 1;
        List<int> newDiffs = new List<int>(10);
        newDiffs.Add(0);
        newDiffs.Add(0);
        newDiffs.Add(1);
        newDiffs.Add(1);
        newDiffs.Add(2);
        newDiffs.Add(2);
        newDiffs.Add(diff);
        newDiffs.Add(diff);
        newDiffs.Add(diff);
        newDiffs.Add(diff);

        return newDiffs[Random.Range(0, newDiffs.Count)];
    }
}
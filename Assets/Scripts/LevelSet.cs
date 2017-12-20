using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSet : MonoBehaviour
{
    [HideInInspector]
    public Level easyLevel;
    [HideInInspector]
    public Level normalLevel;
    [HideInInspector]
    public Level hardLevel;

    public Text easyText;
    public Text normalText;
    public Text hardText;

    void Start()
    {
        if (GameManager.instance.hideoutFlag)
        {
            CreateNewLevelSet();
        }
    }
    void CreateNewLevelSet()
    {
        easyLevel = new Level(0);
        normalLevel = new Level(1);
        hardLevel = new Level(2);

        DisplaySet();
    }

    void DisplaySet()
    {
        string itemText, enemyText;
        DifficultyToText(easyLevel.itemQuantity, easyLevel.enemyQuantity, out itemText, out enemyText);
        easyText.text = "EASY\nFloors: " + easyLevel.floorQuantity + "\nBoardSize: " + easyLevel.boardSize[0] + " x " + easyLevel.boardSize[1] + "\nItems: " + itemText + "\nEnemies: " + enemyText;

        DifficultyToText(normalLevel.itemQuantity, normalLevel.enemyQuantity, out itemText, out enemyText);
        normalText.text = "NORMAL\nFloors: " + normalLevel.floorQuantity + "\nBoardSize: " + normalLevel.boardSize[0] + " x " + normalLevel.boardSize[1] + "\nItems: " + itemText + "\nEnemies: " + enemyText;

        DifficultyToText(hardLevel.itemQuantity, hardLevel.enemyQuantity, out itemText, out enemyText);
        hardText.text = "HARD\nFloors: " + hardLevel.floorQuantity + "\nBoardSize: " + hardLevel.boardSize[0] + " x " + hardLevel.boardSize[1] + "\nItems: " + itemText + "\nEnemies: " + enemyText;
    }

    void DifficultyToText(int item, float enemy, out string itemText, out string enemyText)
    {
        string itemT = "", enemyT = "";
        if (item == 0)
            itemT = "Plentiful";
        else if (item == 1)
            itemT = "Average";
        else if (item == 2)
            itemT = "Sparse";
        
        if (enemy == 3.0f)
            enemyT = "Few";
        else if (enemy == 2.0f)
            enemyT = "Average";
        else if (enemy == 1.5f)
            enemyT = "Many";

        itemText = itemT;
        enemyText = enemyT;
    }

    public void LevelEasy()
    {
        GameManager.instance.StartNewLevel(easyLevel);
    }

    public void LevelNormal()
    {
        GameManager.instance.StartNewLevel(normalLevel);
    }

    public void LevelHard()
    {
        GameManager.instance.StartNewLevel(hardLevel);
    }
}

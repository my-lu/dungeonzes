using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject food;
    public GameObject water;

    int maxInventorySize = 8;
    public List<GameObject> inventoryContents;
    public List<int> inventoryCount;

    void Start()
    {
        inventoryContents = GameManager.instance.invContents;
        inventoryCount = GameManager.instance.invCounts;
    }

    void OnDisable()
    {
        GameManager.instance.invContents = inventoryContents;
        GameManager.instance.invCounts = inventoryCount;
    }

    void Update()
    {
        for(int i = 0; i < inventoryCount.Count; i++)
        {
            if(inventoryCount[i] <= 0)
            {
                inventoryContents.RemoveAt(i);
                inventoryCount.RemoveAt(i);
                GameManager.instance.UpdateInventoryUI();
            }
        }

        GameManager.instance.UpdateInventoryUI();
    }

    public void UseFromInventory(int id)
    {
        GameObject item = inventoryContents[id];
        if (item.tag == "Food")
        {
            if (GameManager.instance.UseFoodSupply(item))
            {
                inventoryCount[id]--;
            }
        }
        else if (item.tag == "Water")
        {
            if (GameManager.instance.UseWaterSupply(item))
            {
                inventoryCount[id]--;
            }
        }
        else if (item.tag == "Sword" || item.tag == "Pickaxe")
        {
            inventoryCount[id]--;
            GameManager.instance.SendToCharacter(item);
        }
    }

    public bool AddToInventory(GameObject item)
    {
        for(int i = 0; i < inventoryContents.Count; i++)
        {
            if(inventoryContents[i] == item)
            {
                inventoryCount[i]++;
                return true;
            }
        }

        if (inventoryContents.Count >= maxInventorySize)
        {
            return false;
        }

        if (item.tag == "Sword" || item.tag == "Pickaxe")
        {
            DontDestroyOnLoad(item);
        }
        inventoryContents.Add(item);
        inventoryCount.Add(1);
        return true;
    }
}

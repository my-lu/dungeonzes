using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{
    public bool stashIndicator;
    public GameObject food;
    public GameObject water;

    public GameObject swordPrefab;
    GameObject swordObject;

    public GameObject pickaxePrefab;
    GameObject pickaxeObject;

    int foodAmount = 2;
    int waterAmount = 2;
    float swordProbability = 0.1f;
    float pickaxeProbability = 0.1f;

    int maxChestSize = 4;
    public List<GameObject> chestContents;
    public List<int> contentsCount;

    void Start()
    {
        if (stashIndicator)
        {
            chestContents = GameManager.instance.stashContents;
            contentsCount = GameManager.instance.stashCounts;
        }
        else if (!stashIndicator)
        {
            MakeNewChest();
        }
    }
    
    void OnDisable()
    {
        if (stashIndicator)
        {
            GameManager.instance.stashContents = chestContents;
            GameManager.instance.stashCounts = contentsCount;
        }
    }

    void Update()
    {
        for(int i = 0; i < contentsCount.Count; i++)
        {
            if(contentsCount[i] <= 0)
            {
                chestContents.RemoveAt(i);
                contentsCount.RemoveAt(i);
                GameManager.instance.DisplayChest(this);
            }
        }

        if(chestContents.Count == 0 && !stashIndicator)
        {
            gameObject.SetActive(false);
        }
    }

    void MakeNewChest()
    {
        List<GameObject> chestList = new List<GameObject>();
        List<int> countList = new List<int>();
        int waterMin = 0;

        int foodCount = Random.Range(0, foodAmount + 1);
        if(foodCount != 0)
        {
            chestList.Add(food);
            countList.Add(foodCount);
        }
        else if(foodCount == 0)
        {
            waterMin = 1;
        }

        int waterCount = Random.Range(waterMin, waterAmount + 1);
        if (waterCount != 0)
        {
            chestList.Add(water);
            countList.Add(waterCount);
        }

        if (Random.Range(0.0f, 1.0f) < swordProbability)
        {
            swordObject = Instantiate(swordPrefab) as GameObject;
            chestList.Add(swordObject);
            countList.Add(1);
        }
        if (Random.Range(0.0f, 1.0f) < pickaxeProbability)
        {
            pickaxeObject = Instantiate(pickaxePrefab) as GameObject;
            chestList.Add(pickaxeObject);
            countList.Add(1);
        }

        chestContents = chestList;
        contentsCount = countList;
    }

    public void CollectFromChest(int id)
    {
        GameObject item = chestContents[id];
        if (item.tag == "Food")
        {
            if (GameManager.instance.ReturnToInventory(item))
            {
                contentsCount[id]--;
            }
        }
        else if(item.tag == "Water")
        {
            if (GameManager.instance.ReturnToInventory(item))
            {
                contentsCount[id]--;
            }
        }
        else if(item.tag == "Sword" || item.tag == "Pickaxe")
        {
            if (GameManager.instance.ReturnToInventory(item))
            {
                contentsCount[id]--;
            }
        }
    }

    public bool AddToChest(GameObject item)
    {
        for(int i = 0; i < chestContents.Count; i++)
        {
            if(chestContents[i] == item)
            {
                contentsCount[i]++;
                return true;
            }
        }

        if (chestContents.Count >= maxChestSize)
        {
            return false;
        }

        if (stashIndicator && (item.tag == "Sword" || item.tag == "Pickaxe"))
        {
            DontDestroyOnLoad(item);
        }
        chestContents.Add(item);
        contentsCount.Add(1);
        return true;
    }
}

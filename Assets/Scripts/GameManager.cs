using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Player player;

    public float levelStartDelay = 2.0f;
    public float turnDelay = 0.05f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    [HideInInspector]
    public bool playersTurn = true;
    public bool inChestUI = false;
    
    GameObject levelImage;
    Text levelText;

    Chest currentChest;
    GameObject chestUI;
    GameObject[] chestButtons;
    GameObject[] chestTexts;

    CharacterPanel loadout;
    public GameObject helmet;
    public GameObject chest;
    public GameObject pants;
    public GameObject boots;
    public Sword sword;
    public Pickaxe pickaxe;

    public List<GameObject> stashContents;
    public List<int> stashCounts;

    Inventory inventory;
    public List<GameObject> invContents;
    public List<int> invCounts;
    GameObject[] inventoryButtons;
    GameObject[] inventoryTexts;

    GameObject newLevelSelect;

    public Enemy playerAttackTarget;
    public GameObject targetingImage;

    List<Enemy> enemies;
    bool enemiesMoving;
    bool doingSetup = true;
    int area = 0;
    int maxArea = 5;
    [HideInInspector]
    public int areaDifficulty = 1;
    [HideInInspector]
    public bool hideoutFlag;

    public int playerHealthPoints = 10;
    public int playerFoodPoints = 100;
    public int playerWaterPoints = 100;

    int pointsPerFoodSupply = 25;
    int pointsPerWaterSupply = 25;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        stashContents = new List<GameObject>();
        stashCounts = new List<int>();
        invContents = new List<GameObject>();
        invCounts = new List<int>();
        
        boardScript = GetComponent<BoardManager>();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        instance.area++;
        if(instance.area > instance.maxArea)
        {
            instance.area = 0;
        }
        instance.InitArea();
    }

    void InitArea()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();

        chestUI = GameObject.Find("ChestUI");
        chestButtons = new GameObject[4];
        chestButtons[0] = GameObject.Find("ChestButton1");
        chestButtons[1] = GameObject.Find("ChestButton2");
        chestButtons[2] = GameObject.Find("ChestButton3");
        chestButtons[3] = GameObject.Find("ChestButton4");

        chestTexts = new GameObject[4];
        chestTexts[0] = GameObject.Find("ChestCount1");
        chestTexts[1] = GameObject.Find("ChestCount2");
        chestTexts[2] = GameObject.Find("ChestCount3");
        chestTexts[3] = GameObject.Find("ChestCount4");

        CloseChest();

        loadout = GameObject.Find("CharacterPanel").GetComponent<CharacterPanel>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        inventoryButtons = new GameObject[8];
        inventoryButtons[0] = GameObject.Find("InventoryButton1");
        inventoryButtons[1] = GameObject.Find("InventoryButton2");
        inventoryButtons[2] = GameObject.Find("InventoryButton3");
        inventoryButtons[3] = GameObject.Find("InventoryButton4");
        inventoryButtons[4] = GameObject.Find("InventoryButton5");
        inventoryButtons[5] = GameObject.Find("InventoryButton6");
        inventoryButtons[6] = GameObject.Find("InventoryButton7");
        inventoryButtons[7] = GameObject.Find("InventoryButton8");

        inventoryTexts = new GameObject[8];
        inventoryTexts[0] = GameObject.Find("InventoryCount1");
        inventoryTexts[1] = GameObject.Find("InventoryCount2");
        inventoryTexts[2] = GameObject.Find("InventoryCount3");
        inventoryTexts[3] = GameObject.Find("InventoryCount4");
        inventoryTexts[4] = GameObject.Find("InventoryCount5");
        inventoryTexts[5] = GameObject.Find("InventoryCount6");
        inventoryTexts[6] = GameObject.Find("InventoryCount7");
        inventoryTexts[7] = GameObject.Find("InventoryCount8");

        GetComponent<DamageController>().Initialize();

        newLevelSelect = GameObject.Find("LevelSelect");
        newLevelSelect.SetActive(false);

        targetingImage = GameObject.Find("Target");
        targetingImage.SetActive(false);

        levelText.text = "Area " + area;
        if (area == 0)
            levelText.text = "Hideout";
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        if (area == 0)
        {
            boardScript.SetupHideout();
            hideoutFlag = true;
        }
        else
        {
            boardScript.SetupScene(area);
            hideoutFlag = false;
        }
    }

    void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "You died.";
        levelImage.SetActive(true);
        enabled = false;
    }

    void Update()
    {
        if (playersTurn || enemiesMoving || doingSetup)
            return;

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    public void RemoveEnemyFromList(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if(enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }

    public void CollectFromChest(int id)
    {
        currentChest.CollectFromChest(id);
        UpdateChestUI();
    }

    public void SendToCharacter(GameObject item)
    {
        loadout.EquipItem(item);
    }

    public bool AddToChest(GameObject item)
    {
        bool x = currentChest.AddToChest(item);
        if (x)
            UpdateChestUI();
        else
            return false;
        return true;
    }

    public void DisplayChest(Chest someChest)
    {
        instance.inChestUI = true;
        if (currentChest == null)
        {
            currentChest = someChest;
            chestUI.SetActive(true);
        }
        UpdateChestUI();
    }

    void UpdateChestUI()
    {
        for (int i = 0; i < currentChest.chestContents.Count; i++)
        {
            chestButtons[i].SetActive(true);
            chestButtons[i].GetComponent<Image>().sprite = currentChest.chestContents[i].GetComponent<SpriteRenderer>().sprite;
            chestTexts[i].GetComponent<Text>().text = currentChest.contentsCount[i].ToString();
        }

        for(int i = currentChest.chestContents.Count; i < 4; i++)
        {
            chestButtons[i].SetActive(false);
        }
    }

    public void CloseChest()
    {
        currentChest = null;
        instance.inChestUI = false;
        foreach (GameObject t in chestButtons)
        {
            t.SetActive(false);
        }
        chestUI.SetActive(false);
    }

    public void InitiateLevelSelect()
    {
        newLevelSelect.SetActive(true);
    }

    public void StartNewLevel(Level level)
    {
        maxArea = level.floorQuantity;
        areaDifficulty = level.difficulty;
        boardScript.UpdateLevel(level);
        SceneManager.LoadScene(1);
    }

    public void TargetEnemy(Enemy enemy)
    {
        playerAttackTarget = enemy;
        targetingImage.transform.position = playerAttackTarget.transform.position;
        targetingImage.SetActive(true);
    }

    public void UntargetEnemy()
    {
        targetingImage.SetActive(false);
        playerAttackTarget = null;
    }

    public void ReportDamage(string damage, Transform location)
    {
        GetComponent<DamageController>().CreateFloatingText(damage, location);
    }

    public bool UseFoodSupply(GameObject food)
    {
        if (instance.inChestUI)
        {
            if (AddToChest(food))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        player.GainFood(pointsPerFoodSupply);
        return true;
    }

    public bool UseWaterSupply(GameObject water)
    {
        if (instance.inChestUI)
        {
            if (AddToChest(water))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        player.GainWater(pointsPerWaterSupply);
        return true;
    }

    public void UseFromInventory(int id)
    {
        inventory.UseFromInventory(id);
        UpdateInventoryUI();
    }

    public bool ReturnToInventory(GameObject item)
    {
        bool x = inventory.AddToInventory(item);
        if (x)
            UpdateInventoryUI();
        else
            return false;
        return true;
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < inventory.inventoryContents.Count; i++)
        {
            inventoryButtons[i].SetActive(true);
            inventoryButtons[i].GetComponent<Image>().sprite = inventory.inventoryContents[i].GetComponent<SpriteRenderer>().sprite;
            inventoryTexts[i].GetComponent<Text>().text = inventory.inventoryCount[i].ToString();
        }

        for (int i = inventory.inventoryContents.Count; i < 8; i++)
        {
            inventoryButtons[i].SetActive(false);
        }
    }

    public void DisplayLoadoutInfo(int id)
    {
        loadout.DisplayLoadoutInfo(id);
    }
}
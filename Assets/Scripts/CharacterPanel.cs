using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    int baseAttackDamage = 0;
    int baseWallDamage = 1;
    float moveSpeed = 1000.0f;
    RectTransform rectTransform;
    Vector2 openPosition = new Vector2(0.0f, -95.0f);
    Vector2 closePosition = new Vector2(0.0f, -600.0f);
    bool location;

    public GameObject helmetSlotUI;
    public GameObject chestSlotUI;
    public GameObject pantsSlotUI;
    public GameObject bootsSlotUI;
    public GameObject swordSlotUI;
    public GameObject pickaxeSlotUI;

    public GameObject helmetSlot;
    public GameObject chestSlot;
    public GameObject pantsSlot;
    public GameObject bootsSlot;
    public Sword swordSlot;
    public Pickaxe pickaxeSlot;

    GameObject itemInfoPanel;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        location = false;

        helmetSlot = GameManager.instance.helmet;
        chestSlot = GameManager.instance.chest;
        pantsSlot = GameManager.instance.pants;
        bootsSlot = GameManager.instance.boots;
        swordSlot = GameManager.instance.sword;
        pickaxeSlot = GameManager.instance.pickaxe;

        itemInfoPanel = GameObject.Find("ItemInfoPanel");
    }

    void OnDisable()
    {
        GameManager.instance.helmet = helmetSlot;
        GameManager.instance.chest = chestSlot;
        GameManager.instance.pants = pantsSlot;
        GameManager.instance.boots = bootsSlot;
        GameManager.instance.sword = swordSlot;
        GameManager.instance.pickaxe = pickaxeSlot;
    }

    void Update()
    {
        if (location == true && rectTransform.anchoredPosition != openPosition)
        {
            rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, openPosition, moveSpeed * Time.deltaTime);
        }
        if (location == false && rectTransform.anchoredPosition != closePosition)
        {
            rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, closePosition, moveSpeed * Time.deltaTime);
        }

        UpdateCharacterUI();
    }

    public void CharacterButton()
    {
        if (rectTransform.anchoredPosition == openPosition)
        {
            location = false;
            itemInfoPanel.GetComponent<ItemInfoPanel>().DelayClosePanel();
        }
        else if (rectTransform.anchoredPosition == closePosition)
        {
            location = true;
        }
    }

    public int GetAttackDamage()
    {
        if (swordSlot != null)
        {
            return swordSlot.Damage + baseAttackDamage;
        }
        return baseAttackDamage;
    }

    public int GetWallDamage()
    {
        if(pickaxeSlot != null)
        {
            return pickaxeSlot.Damage + baseWallDamage;
        }
        return baseWallDamage;
    }

    public void EquipItem(GameObject item)
    {
        if (item.tag == "Sword")
        {
            if (swordSlot == null)
            {
                swordSlot = item.GetComponent<Sword>();
                DontDestroyOnLoad(swordSlot.gameObject);
            }
            else
            {
                if (GameManager.instance.ReturnToInventory(swordSlot.gameObject))
                {
                    swordSlot = item.GetComponent<Sword>();
                    DontDestroyOnLoad(swordSlot.gameObject);
                }
                else
                {
                    GameManager.instance.ReturnToInventory(item);
                }
            }
        }
        else if(item.tag == "Pickaxe")
        {
            if (pickaxeSlot == null)
            {
                pickaxeSlot = item.GetComponent<Pickaxe>();
                DontDestroyOnLoad(pickaxeSlot.gameObject);
            }
            else
            {
                if (GameManager.instance.ReturnToInventory(pickaxeSlot.gameObject))
                {
                    pickaxeSlot = item.GetComponent<Pickaxe>();
                    DontDestroyOnLoad(pickaxeSlot.gameObject);
                }
                else
                {
                    GameManager.instance.ReturnToInventory(item);
                }
            }
        }
    }

    void UpdateCharacterUI()
    {
        if (helmetSlot != null)
            helmetSlotUI.GetComponent<Image>().sprite = helmetSlot.GetComponent<SpriteRenderer>().sprite;
        if (chestSlot != null)
            chestSlotUI.GetComponent<Image>().sprite = chestSlot.GetComponent<SpriteRenderer>().sprite;
        if (pantsSlot != null)
            pantsSlotUI.GetComponent<Image>().sprite = pantsSlot.GetComponent<SpriteRenderer>().sprite;
        if (bootsSlot != null)
            bootsSlotUI.GetComponent<Image>().sprite = bootsSlot.GetComponent<SpriteRenderer>().sprite;
        if (swordSlot != null)
            swordSlotUI.GetComponent<Image>().sprite = swordSlot.GetComponent<SpriteRenderer>().sprite;
        if (pickaxeSlot != null)
            pickaxeSlotUI.GetComponent<Image>().sprite = pickaxeSlot.GetComponent<SpriteRenderer>().sprite;
    }

    public void DisplayLoadoutInfo(int id)
    {
        if (id == 1)
        {
            if (helmetSlot != null)
                Debug.Log(helmetSlot.name);
        }
        else if (id == 2)
        {
            if (chestSlot != null)
                Debug.Log(chestSlot.name);
        }
        else if (id == 3)
        {
            if (pantsSlot != null)
                Debug.Log(pantsSlot.name);
        }
        else if (id == 4)
        {
            if (bootsSlot != null)
                Debug.Log(bootsSlot.name);
        }
        else if (id == 5)
        {
            if (swordSlot != null)
                itemInfoPanel.GetComponent<ItemInfoPanel>().DisplaySwordInfo(swordSlot);
        }
        else if (id == 6)
        {
            if (pickaxeSlot != null)
                itemInfoPanel.GetComponent<ItemInfoPanel>().DisplayPickaxeInfo(pickaxeSlot);
        }
    }
}

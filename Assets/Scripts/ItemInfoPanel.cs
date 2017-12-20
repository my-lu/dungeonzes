using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPanel : MonoBehaviour
{
    public Image itemImage;
    public Text itemName;
    public Text itemMainStat;
    public Text itemDurability;
    public Text itemModifiers;

    float moveSpeed = 1000.0f;
    float moveDelay = 0.25f;
    RectTransform rectTransform;
    Vector2 openPosition = new Vector2(350.0f, 0.0f);
    Vector2 closePosition = new Vector2(350.0f, -240.0f);
    bool location;

    GameObject helmetHolder;
    GameObject chestHolder;
    GameObject pantsHolder;
    GameObject bootsHolder;
    Sword swordHolder;
    Pickaxe pickaxeHolder;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        location = false;
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
    }

    public void DelayClosePanel()
    {
        Invoke("ClosePanel", 1.0f);
    }

    void ClosePanel()
    {
        location = false;
    }

    void OpenPanel()
    {
        location = true;
    }

    public void DisplaySwordInfo(Sword sword)
    {
        ClosePanel();

        swordHolder = sword;
        Invoke("UpdateSwordInfo", moveDelay);
        Invoke("OpenPanel", moveDelay);
    }

    void UpdateSwordInfo()
    {
        itemImage.sprite = swordHolder.GetComponent<SpriteRenderer>().sprite;
        itemName.text = swordHolder.SwordName;
        itemMainStat.text = "Damage: " + swordHolder.Damage;
        itemDurability.text = "Durability: " + swordHolder.Durability;

        itemModifiers.text = "Modifiers: None";
    }

    public void DisplayPickaxeInfo(Pickaxe pickaxe)
    {
        ClosePanel();

        pickaxeHolder = pickaxe;
        Invoke("UpdatePickaxeInfo", moveDelay);
        Invoke("OpenPanel", moveDelay);
    }

    void UpdatePickaxeInfo()
    {
        itemImage.sprite = pickaxeHolder.GetComponent<SpriteRenderer>().sprite;
        itemName.text = pickaxeHolder.PickaxeName;
        itemMainStat.text = "Damage: " + pickaxeHolder.Damage;
        itemDurability.text = "Durability: " + pickaxeHolder.Durability;

        itemModifiers.text = "Modifiers: None";
    }
}

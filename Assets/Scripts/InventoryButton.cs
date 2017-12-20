using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    int id;

    void Start()
    {
        id = Int32.Parse(name[15].ToString()) - 1;
    }

    public void InvPressed()
    {
        GameManager.instance.UseFromInventory(id);
    }
}

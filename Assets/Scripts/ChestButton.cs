using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestButton : MonoBehaviour
{
    int id;

    void Start()
    {
        id = Int32.Parse(name[11].ToString()) - 1;
    }

    public void ChestPressed()
    {
        GameManager.instance.CollectFromChest(id);
    }
}

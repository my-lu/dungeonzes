using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanelButton : MonoBehaviour
{
    int id;

    void Start()
    {
        id = Int32.Parse(name[0].ToString());
    }

    public void LoadoutPressed()
    {
        GameManager.instance.DisplayLoadoutInfo(id);
    }
}

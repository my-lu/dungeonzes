using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pickaxe : MonoBehaviour
{
    int damage;
    int durability;
    string pickaxeName;

    void Start()
    {
        CreatePickaxe();
    }

    void CreatePickaxe()
    {
        damage = 1;
        durability = 100;
        pickaxeName = "Pickaxe " + Random.Range(0, 100).ToString();
    }

    public int Damage { get { return damage; } }
    public int Durability { get { return durability; } }
    public string PickaxeName { get { return pickaxeName; } }
}
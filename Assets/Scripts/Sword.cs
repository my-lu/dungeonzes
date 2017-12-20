using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sword : MonoBehaviour
{
    int damage;
    int durability;
    string swordName;

    void Start()
    {
        CreateSword();
    }

    void CreateSword()
    {
        damage = 1;
        durability = 100;
        swordName = "Sword " + Random.Range(0, 100).ToString();
    }

    public int Damage { get { return damage; } }
    public int Durability { get { return durability; } }
    public string SwordName { get { return swordName; } }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Player : MovingObject
{
    public float restartLevelDelay = 1.0f;
    float attackDelay = 0.15f;

    public Text healthText;
    public Text foodText;
    public Text waterText;

    int health;
    int food;
    int water;

    public CharacterPanel loadout;

    protected override void Start()
    {
        health = GameManager.instance.playerHealthPoints;
        food = GameManager.instance.playerFoodPoints;
        water = GameManager.instance.playerWaterPoints;
        UpdatePoints();

        base.Start();
    }

    void OnDisable()
    {
        GameManager.instance.playerHealthPoints = health;
        GameManager.instance.playerFoodPoints = food;
        GameManager.instance.playerWaterPoints = water;
    }

    void Update()
    {
        if (!GameManager.instance.playersTurn)
            return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (!GameManager.instance.hideoutFlag)
        {
            LoseFood(1);
            LoseWater(1);
        }
        UpdatePoints();

        base.AttemptMove<T>(xDir, yDir);

        //RaycastHit2D hit;

        CheckIfGameOver();

        //Check for enemies
        Invoke("CheckForEnemies", attackDelay);

        GameManager.instance.playersTurn = false;
    }

    void CheckForEnemies()
    {
        List<Enemy> neighboringEnemies = new List<Enemy>();

        RaycastHit2D hit;
        bool foundEnemy = AttemptAttack(-1, 0, out hit);
        if (foundEnemy && hit.transform.GetComponent<Enemy>() != null)
            neighboringEnemies.Add(hit.transform.GetComponent<Enemy>());

        foundEnemy = AttemptAttack(0, 1, out hit);
        if (foundEnemy && hit.transform.GetComponent<Enemy>() != null)
            neighboringEnemies.Add(hit.transform.GetComponent<Enemy>());

        foundEnemy = AttemptAttack(1, 0, out hit);
        if (foundEnemy && hit.transform.GetComponent<Enemy>() != null)
            neighboringEnemies.Add(hit.transform.GetComponent<Enemy>());

        foundEnemy = AttemptAttack(0, -1, out hit);
        if (foundEnemy && hit.transform.GetComponent<Enemy>() != null)
            neighboringEnemies.Add(hit.transform.GetComponent<Enemy>());

        if (neighboringEnemies.Count == 0)
            return;

        Enemy currentEnemy = GameManager.instance.playerAttackTarget;
        if (currentEnemy != null && neighboringEnemies.Contains(currentEnemy))
        {
            currentEnemy.LoseHealth(GetAttackDamage());
        }
        else
        {
            neighboringEnemies[Random.Range(0, neighboringEnemies.Count)].LoseHealth(GetAttackDamage());
        }

        GameManager.instance.UntargetEnemy();
    }

    int GetAttackDamage()
    {
        return loadout.GetAttackDamage();
    }

    int GetWallDamage()
    {
        return loadout.GetWallDamage();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("LoadArea", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Chest" || other.tag == "Stash")
        {
            GameManager.instance.DisplayChest(other.GetComponent<Chest>());
        }
        else if(other.tag == "HideoutExit")
        {
            Invoke("LoadLevelSelect", restartLevelDelay);
            enabled = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Chest" || other.tag == "Stash")
        {
            GameManager.instance.CloseChest();
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(GetWallDamage());
    }

    private void LoadLevelSelect()
    {
        GameManager.instance.InitiateLevelSelect();
    }

    private void LoadArea()
    {
        SceneManager.LoadScene(1);
    }

    public void LoseHealth(int loss)
    {
        health -= loss;
        GameManager.instance.ReportDamage(loss.ToString(), this.transform);
        UpdatePoints();
        CheckIfGameOver();
    }

    public void GainHealth(int gain)
    {
        health += gain;
        if(health >= 10)
        {
            health = 10;
        }
        UpdatePoints();
    }

    public void LoseFood(int loss)
    {
        food -= loss;
        CheckStatus();
        UpdatePoints();
    }

    public void GainFood(int gain)
    {
        food += gain;
        if(food >= 100)
        {
            food = 100;
        }
        UpdatePoints();
    }

    public void LoseWater(int loss)
    {
        water -= loss;
        CheckStatus();
        UpdatePoints();
    }

    public void GainWater(int gain)
    {
        water += gain;
        if(water >= 100)
        {
            water = 100;
        }
        UpdatePoints();
    }

    void CheckStatus()
    {
        if (food < 0)
        {
            LoseHealth(1);
            food = 0;
        }
        if (water < 0)
        {
            LoseHealth(1);
            water = 0;
        }
        if(food >= 99 && water >= 99)
        {
            GainHealth(1);
        }
    }

    void CheckIfGameOver()
    {
        if (health <= 0)
            GameManager.instance.GameOver();
    }

    void UpdatePoints()
    {
        healthText.text = "Health: " + health;
        foodText.text = "Food: " + food;
        waterText.text = "Water: " + water;
    }
}
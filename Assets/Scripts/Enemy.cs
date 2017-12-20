using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    int hp;
    int attackDamage;

    Transform target;
    bool skipMove;

    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        CalculateStats();

        base.Start();
    }

    void CalculateStats()
    {
        hp = 3;
        attackDamage = 1;
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;

        hitPlayer.LoseHealth(attackDamage);
    }

    void OnMouseDown()
    {
        GameManager.instance.TargetEnemy(this);
    }

    public void LoseHealth(int loss)
    {
        hp -= loss;
        GameManager.instance.ReportDamage(loss.ToString(), this.transform);
        if (hp <= 0)
        {
            gameObject.SetActive(false);
            GameManager.instance.RemoveEnemyFromList(this);
        }
    }
}
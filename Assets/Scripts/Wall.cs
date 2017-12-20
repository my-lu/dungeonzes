using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite dmgSprite;
    public int hp = 10;

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int loss)
    {
        hp -= loss;
        GameManager.instance.ReportDamage(loss.ToString(), this.transform);

        if (hp <= 5)
        {
            spriteRenderer.sprite = dmgSprite;
        }

        if(hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}

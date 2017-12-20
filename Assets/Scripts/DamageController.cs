using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageController : MonoBehaviour
{
    public DamageReport damageText;
    GameObject canvas;

    public void Initialize()
    {
        canvas = GameObject.Find("Canvas");
    }

    public void CreateFloatingText(string text, Transform location)
    {
        if (text == "0")
            return;

        DamageReport instance = Instantiate(damageText);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + Random.Range(-0.25f, 0.25f), location.position.y + Random.Range(-0.25f, 0.25f)));
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPosition;
        instance.SetText(text);
    }
}

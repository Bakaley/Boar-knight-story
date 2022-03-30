using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCage : SparkReceiver
{

    SpriteRenderer rend;
    BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rend = GetComponent<SpriteRenderer>();
    }

    protected override void activate()
    {
        rend.color = new Color(1, 1, 1, .25f);
        boxCollider.enabled = false;
    }

    protected override void deactivate()
    {
        rend.color = new Color(1, 1, 1, 1);
        boxCollider.enabled = true;
    }

}

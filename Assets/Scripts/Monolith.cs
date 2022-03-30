using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monolith : SparkSender, ISparkable
{
    [SerializeField]
    Sprite activatedState;

    public void sparkActivate()
    {
        switchOn();
        GetComponent<SpriteRenderer>().sprite = activatedState;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class SparkSender : MonoBehaviour
{
    [SerializeField]
    Tilemap wires;
    [SerializeField]
    Color enabledWiresColor;
    [SerializeField]
    Color disabledWiresColor;

    public bool sparking
    {
        get; private set;
    } = false;

    public event EventHandler <bool> SparkChanging;

    protected void switchOn()
    {
        wires.color = enabledWiresColor;
        sparking = true;
        SparkChanging?.Invoke(this, sparking);
    }

    protected void switchOff()
    {
        wires.color = disabledWiresColor;
        sparking = false;
        SparkChanging?.Invoke(this, sparking);
    }

}

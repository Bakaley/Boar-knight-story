using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class SparkReceiver : MonoBehaviour
{
    [SerializeField]
    SparkSender[] senders;

    private void Start()
    {
        foreach (SparkSender sender in senders)
        {
            sender.SparkChanging += SparkHandler;
        }
    }

    protected abstract void activate();
    protected abstract void deactivate();

    void SparkHandler (object sender, bool newValue)
    {
        foreach (SparkSender sparkSender in senders)
        {
            if (!sparkSender.sparking)
            {
                deactivate();
                return;
            }
        }
        activate();
    }

    private void OnDestroy()
    {
        foreach (SparkSender sparkSender in senders)
        {
            sparkSender.SparkChanging -= SparkHandler;
        }
    }
}

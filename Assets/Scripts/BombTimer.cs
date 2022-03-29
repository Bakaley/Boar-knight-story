using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


//������ �� ������������ ������ �� ����� ������� �� ������������ ��� �������� gameObjecte
public class BombTimer : MonoBehaviour
{

    int explosionTimer;
    TextMeshPro textTimer;


    private void Awake()
    {
        textTimer = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        explosionTimer = GetComponentInParent<Bomb>().BeginningBombTimer;
        StartCoroutine("countDown");
    }

    //������������ ��� timerRefresh
    int currentTimer;

    IEnumerator countDown()
    {
        for (int i = explosionTimer; i > 0; i--)
        {
            GetComponent<Animation>().Play();
            currentTimer = i;
            yield return new WaitForSeconds(1);
        }
        onTimerEnd?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler onTimerEnd;

    public void timerRefresh()
    {
        textTimer.SetText(currentTimer + "");
    }
}

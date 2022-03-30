using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject hpContainer;
    [SerializeField]
    GameObject heart;
    [SerializeField]
    Player player;

    List<GameObject> hearts
    {
        get
        {
            List<GameObject> list = new List<GameObject>();
            foreach (Transform tr in hpContainer.transform)
            {
                if (tr.gameObject != hpContainer) list.Add(tr.gameObject);
            }
            return list;
        }
    }

    private void Start()
    {
        while (hearts.Count > player.StartHP) Destroy(hearts[0]);
        while (hearts.Count < player.StartHP) Instantiate(heart, hpContainer.transform);
        player.OnHPchange += changeHPHandler;
    }

    //hp не может уменьшиться или увеличиться за раз больше, чем на 1, поэтому делать цикл смысла нет
    void changeHPHandler(object sender, int diff)
    {
        if (diff < 0) Destroy(hearts[0]);
        else Instantiate(heart, hpContainer.transform);
    }
}

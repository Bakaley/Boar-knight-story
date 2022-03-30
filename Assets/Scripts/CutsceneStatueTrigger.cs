using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CutsceneStatueTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject monster;
    [SerializeField]
    GameObject alertSymbol;
    [SerializeField]
    PushableObstacle obst;
    public void flipAlertTrigger (object sender, Vector2 dir){
        monster.GetComponent<SpriteRenderer>().flipX = true;
        alertSymbol.SetActive(true);
        Destroy(this);
    }

    private void Start()
    {
        obst.OnPushStart += flipAlertTrigger;
    }

    private void OnDestroy()
    {
        obst.OnPushStart -= flipAlertTrigger;
    }
}

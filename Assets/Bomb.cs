using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bomb : MonoBehaviour
{
    
    //заспавненная бомба не должна выдавливать игрока из клетки, пока он не выйдет из клетки сам
    [SerializeField]
    GameObject defaultCollder;

    //выход игрока из клетки с заспавненной бомбой включает коллизию с этой бомбой
    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Player")) defaultCollder.gameObject.layer = 6;
    }


}

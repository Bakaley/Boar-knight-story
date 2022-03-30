using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FloorButton : SparkSender
{
    //список состояний, в котором может быть игрок
    int[] layersActivating = { 6, 9, 10, 11 };
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (layersActivating.Contains(collision.gameObject.layer))
        {
            switchOn();
            return;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 9)
        {
            switchOff();
            return;
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;


public class Bomb : MonoBehaviour
{

    [SerializeField]
    GameObject centralExposionSampler;
    [SerializeField]
    GameObject sideExposionSampler;

    [SerializeField]
    int explosionTimer;
    

    public int BeginningBombTimer
    {
        get
        {
            return explosionTimer;
        }
    }

    //заспавненная бомба не должна выдавливать игрока из клетки, пока он не выйдет из клетки сам
    [SerializeField]
    GameObject defaultCollider;

    BombTimer bombTimer;

    private void Awake()
    {
        bombTimer = GetComponentInChildren<BombTimer>();
        bombTimer.onTimerEnd += timerEndHandler;
    }

    //выход игрока из клетки с заспавненной бомбой включает коллизию с этой бомбой
    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Player")) defaultCollider.gameObject.layer = 6;
    }

    void timerEndHandler(object sender, EventArgs args)
    {
        Tilemap obstacles = TilemapManager.StaticInstance.Obstacles;
        Tilemap bounds = TilemapManager.StaticInstance.Walls;
        Tilemap columns = TilemapManager.StaticInstance.Columns;

        Vector3Int currentCell = obstacles.WorldToCell(gameObject.transform.position);

        Explose(currentCell, Vector3Int.zero, 0);
        Explose(currentCell, new Vector3Int(1, 0, 0), 0);
        Explose(currentCell, new Vector3Int(-1, 0, 0), 180);
        Explose(currentCell, new Vector3Int(0, 1, 0), 90);
        Explose(currentCell, new Vector3Int(0, -1, 0), 270);
        

        void Explose (Vector3Int currentCell, Vector3Int offset, int angle)
        {
            currentCell += offset;
            //central для места бомбы и side для клеток вокруг
            GameObject exploseSampler = offset == (Vector3.zero) ? centralExposionSampler : sideExposionSampler;
            //взрыв не происходит, если в этой клетке колонна или стена
            if (columns.GetTile(currentCell) == null && bounds.GetTile(currentCell) == null)
            {
                //наносим урон в клетку
                bool damageWasDealt = TilemapManager.StaticInstance.dealDamage(currentCell);
                if (damageWasDealt)
                {

                }

                Vector2 worldPos = obstacles.GetCellCenterWorld(currentCell);
                //кастуем BoxCollider
                int mask = ~(LayerMask.GetMask("Ignore Raycast"));
                RaycastHit2D[] raycastHits = Physics2D.BoxCastAll(new Vector2(worldPos.x, worldPos.y), new Vector2(.95f, .95f), 0, Vector2.zero, float.PositiveInfinity, mask);
                foreach (RaycastHit2D hit in raycastHits)
                {
                    if (hit.collider) Debug.Log(hit.collider.gameObject);
                }
                Instantiate(exploseSampler, worldPos, Quaternion.Euler(0, 0, angle));
            }
        }

        Destroy(gameObject);
    }
}

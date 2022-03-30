using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class PushableObstacle : MonoBehaviour, IPushable
{
    new Rigidbody2D rigidbody2D;
    Vector2 movementDirection = Vector2.zero;
    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.bodyType = RigidbodyType2D.Static;
    }

    float speedModifier = 0;
    [SerializeField]
    float pushedSpeedModifier = 3;

    public void push(Vector2 direction)
    {
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        speedModifier = pushedSpeedModifier;
        movementDirection = direction;
        gameObject.layer = 11;
        OnPushStart?.Invoke(this, direction);
    }

    private void FixedUpdate()
    {
        if(speedModifier != 0) rigidbody2D.MovePosition(rigidbody2D.position + movementDirection * Time.fixedDeltaTime * speedModifier);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //статуя наносит урон, только когда двигается
        if(speedModifier != 0)
        {
            IDamagable unit = collision.collider.gameObject.GetComponent<IDamagable>();
            //наносим урон при столкновении
            if (unit != null) unit.sufferDamage();
            //если это не игрок и не монстр, то останавливаемся и становимся в центр клетки
            else
            {
                rigidbody2D.bodyType = RigidbodyType2D.Static;
                speedModifier = 0;
                gameObject.layer = 10;
                //чтобы встать в центр клетки, можно использовать любую Tilemap
                Tilemap map = TilemapManager.StaticInstance.FloorTilemap;
                transform.position = map.GetCellCenterWorld(map.WorldToCell(transform.position));

            }
        }
    }

    public event EventHandler<Vector2> OnPushStart;
}

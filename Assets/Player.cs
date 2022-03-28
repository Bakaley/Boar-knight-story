using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{

    [SerializeField]
    Tilemap tilemap;


    //[SerializeField]
    float speedModifier = 1f;

    Vector2 movementDirection = Vector2.zero;

    SpriteRenderer spriteRenderer;
    new Rigidbody2D rigidbody2D;
    Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //GetAxisRaw возвращает 0, 1 и -1 без промежуточных значений, чтобы герой не скользил при остановке
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (x > 0.01) GetComponent<SpriteRenderer>().flipX = true;
        else if (x < -0.01) GetComponent<SpriteRenderer>().flipX = false;
        
        
        movementDirection = new Vector3(x, y);

        animator.SetFloat("MovementSpeed", movementDirection.magnitude);
    }

    private void FixedUpdate()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + movementDirection * Time.fixedDeltaTime * speedModifier);
    }

    private void LateUpdate()
    {
        // get current grid location
        Vector3Int currentCell = tilemap.WorldToCell(transform.position);

        tilemap.SetTile(currentCell, null);

        // add one in a direction (you'll have to change this to match your directional control)

    }
}

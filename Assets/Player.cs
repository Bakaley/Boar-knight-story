using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField]
    Tilemap tilemap;
    [SerializeField]
    GameObject bomb;

    [SerializeField]
    float speedModifier = 1f;

    Vector2 movementDirection = Vector2.zero;

    SpriteRenderer spriteRenderer;
    new Rigidbody2D rigidbody2D;
    Animator animator;
    PlayerInput playerInput;
    BoxCollider2D collider2d;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        collider2d = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        playerInput.actions["Bomb"].performed += _ =>  spawnBomb();
    }

    void Update()
    {
        movementDirection = playerInput.actions["Move"].ReadValue<Vector2>();
        float x = movementDirection.x;
        float y = movementDirection.y;

        //sprite.flip не флипает 2д коллайдер, поэтому
        void flip()
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        if (x > 0.01 && transform.localScale.x > 0) flip();
        else if (x < -0.01 && transform.localScale.x < 0) flip();

        animator.SetFloat("MovementSpeed", movementDirection.magnitude);

    }

    private void FixedUpdate()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + movementDirection * Time.fixedDeltaTime * speedModifier);
    }


    void spawnBomb()
    {
        Instantiate(bomb, tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.localPosition)), Quaternion.identity);
    }

}

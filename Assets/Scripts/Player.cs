using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using System;

public class Player : MonoBehaviour, IDamagable
{

    [SerializeField]
    Tilemap tilemap;
    [SerializeField]
    GameObject bomb;
    [SerializeField]
    int startBombDamage = 1;
    [SerializeField]
    float speedModifier = 1f;

    [SerializeField]
    int startHP = 3;
    int currentHP = 3;

    public int StartHP
    {
        get => startHP;
    }

    Vector2 movementDirection = Vector2.zero;

    SpriteRenderer spriteRenderer;
    new Rigidbody2D rigidbody2D;
    Animator animator;
    PlayerInput playerInput;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        currentHP = startHP;
        BombDamage = startBombDamage;
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
            if(speedModifier!=0) transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        if (x > 0.01 && transform.localScale.x > 0) flip();
        else if (x < -0.01 && transform.localScale.x < 0) flip();

        animator.SetFloat("MovementSpeed", movementDirection.magnitude * speedModifier);


    }

    private void FixedUpdate()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + movementDirection * Time.fixedDeltaTime * speedModifier);
    }


    void spawnBomb()
    {
        Instantiate(bomb, tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position)), Quaternion.identity);
    }

    public void sufferDamage(int damage)
    {
        //игрок не может потерять больше одного хп за удар
        currentHP--;
        animator.SetInteger("HP", currentHP);
        OnHPchange?.Invoke(this, -1);
        if (currentHP <= 0) die(.5f);
    }

    public void heal()
    {
        currentHP++;
        OnHPchange?.Invoke(this, 1);
    }

    public void die(float time)
    {
        speedModifier = 0;
    }

    public event EventHandler<int> OnHPchange;

    public static int BombDamage
    {
        get; private set;
    }
}

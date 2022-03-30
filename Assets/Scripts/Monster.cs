using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamagable
{
    public enum BEHAVIOR_TYPE
    {
        STANDING,
        HORIZONTAL_PATROL,
        VERTICAL_PATROL,
        RANDOM_DIRECTION
    }

    [SerializeField]
    int startHP = 1;
    [SerializeField]
    BEHAVIOR_TYPE behavior;
    [SerializeField]
    float speedModifier = 1;

    int currentHP;


    SpriteRenderer spriteRenderer;
    new Rigidbody2D rigidbody2D;
    Animator animator;

    //возможные направления движения
    Vector2[] directions = new Vector2[4];
    //текущий вектор движения
    Vector2 movementDirection = Vector2.zero;

    private void Awake()
    {
        directions[0] = Vector2.left;
        directions[1] = Vector2.right;
        directions[2] = Vector2.up;
        directions[3] = Vector2.down;

        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHP = startHP;
    }

    private void Start()
    {
        switch (behavior)
        {
            case BEHAVIOR_TYPE.STANDING:
                speedModifier = 0;
                break;
            case BEHAVIOR_TYPE.HORIZONTAL_PATROL:
                //Random.Range(0, 2) == 0 возвращает true/false с одинаковой вероятностью 
                movementDirection = Random.Range(0, 2) == 0 ? directions[0] : directions[1];
                break;
            case BEHAVIOR_TYPE.VERTICAL_PATROL:
                movementDirection = Random.Range(0, 2) == 0 ? directions[2] : directions[3];
                break;
            case BEHAVIOR_TYPE.RANDOM_DIRECTION:
                movementDirection = directions[Random.Range(0, 4)];
                break;
        }
    }

    private void Update()
    {
        animator.SetFloat("MovementSpeed", speedModifier);
        if (movementDirection.x > 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (movementDirection.x < 0) transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void FixedUpdate()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + movementDirection * Time.fixedDeltaTime * speedModifier);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        //наносим угроку урон при столкновении
        if (collision.collider.gameObject.CompareTag("Player")) collision.collider.gameObject.GetComponent<IDamagable>().sufferDamage();
        //если это не игрок, то разворачиваемся
        else
        {
            movementDirection = movementDirection * -1;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (behavior == BEHAVIOR_TYPE.RANDOM_DIRECTION) movementDirection = directions[Random.Range(0, 4)];


    }

    public void sufferDamage()
    {
        currentHP--;
        animator.SetInteger("HP", currentHP);
        if (currentHP <= 0) die(.5f);
    }

    public void die(float time)
    {
        GetComponent<Collider2D>().enabled = false;
        speedModifier = 0;
        Invoke("DestroyObj", time);
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }

}

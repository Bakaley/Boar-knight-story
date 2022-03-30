using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamagable
{
    public enum BEHAVIOR_TYPE
    {
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

    //��������� ����������� ��������
    Vector2[] directions = new Vector2[4];
    //������� ������ ��������
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
            case BEHAVIOR_TYPE.HORIZONTAL_PATROL:
                //Random.Range(0, 2) == 0 ���������� true/false � ���������� ������������ 
                movementDirection = Random.Range(0, 2) == 0 ? directions[0] : directions[1];
                if (movementDirection.x > 0 && transform.localScale.x > 0) flip();
                else if (movementDirection.x < -0.01 && transform.localScale.x < 0) flip();
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

    }

    private void FixedUpdate()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + movementDirection * Time.fixedDeltaTime * speedModifier);
    }

    void flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        //������� ������ ���� ��� ������������
        if (collision.collider.gameObject.CompareTag("Player")) collision.collider.gameObject.GetComponent<IDamagable>().sufferDamage(1);
        //���� ��� �� �����, �� ���������������
        else
        {
            movementDirection = movementDirection * -1;
            if (movementDirection.x > 0 && transform.localScale.x > 0) flip();
            else if (movementDirection.x < 0 && transform.localScale.x < 0) flip();
        }
    }

    public void sufferDamage(int damage)
    {
        currentHP-= damage;
        animator.SetInteger("HP", currentHP);
        if (currentHP <= 0) die(.5f);
    }

    public void die(float time)
    {
        speedModifier = 0;
        Invoke("DestroyObj", time);
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamagable
{
    
    [SerializeField]
    UIManager ui;
    [SerializeField]
    Tilemap tilemap;
    [SerializeField]
    GameObject bomb;
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

    float invulnerabilityTimer = 1f;
    bool invulnerable = false;
    private void Awake()
    {
        Application.targetFrameRate = 60;

        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        currentHP = startHP;
    }

    private void Start()
    {
        //можно было бы использовать аргумент дискардинг, но тогда мы не сможем отписатьс€
        //а отписыватьс€ надо, т.к. когда мы перезагружаем сцену, прив€зки остаютс€, а экземл€ра игрока уже нет
        playerInput.actions["Bomb"].performed += spawnBomb;
        playerInput.actions["Restart"].performed += restartLevel;

    }

    void Update()
    {
        movementDirection = playerInput.actions["Move"].ReadValue<Vector2>();
        float x = movementDirection.x;
        float y = movementDirection.y;

        //sprite.flip не флипает 2д коллайдер, поэтому поворачиваем весь объект
        //но только пока герой жив
        if(speedModifier != 0)
        {
            if (x > 0.01) transform.rotation = Quaternion.Euler(0, 180, 0);
            else if (x < -0.01) transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        //мен€ем анимацию на движение/покой
        animator.SetFloat("MovementSpeed", movementDirection.magnitude * speedModifier);


    }

    private void FixedUpdate()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + movementDirection * Time.fixedDeltaTime * speedModifier);
    }



    void spawnBomb(InputAction.CallbackContext _)
    {
        //если в этой клетке уже есть бомба, то ничего не делаем
        //посколько бомбы прив€заны к центрам клетки, то castBox надо делать из центра клетки
        //получаем клетку
        Tilemap map = TilemapManager.StaticInstance.FloorTilemap;
        Vector3Int cell = map.WorldToCell(transform.position);
        //получаем центр клетки
        Vector2 worldPos = map.GetCellCenterWorld(cell);
        //все коллайдеры в текущей клетке
        RaycastHit2D[] raycastHits = Physics2D.BoxCastAll(new Vector2(worldPos.x, worldPos.y), new Vector2(.95f, .95f), 0, Vector2.zero);
        foreach (RaycastHit2D hit in raycastHits)
        {
            if (hit.collider.gameObject.CompareTag("Bomb")) return;
        }

        Instantiate(bomb, tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position)), Quaternion.identity);
    }

    public void sufferDamage()
    {
        if (invulnerable) return;
        currentHP--;

        if (currentHP > 0)
        {
            //таймер неу€звимости после получени€ урона
            startInvulnerability(invulnerabilityTimer);
        }
        if(currentHP >= 0)
        {
            animator.SetInteger("HP", currentHP);
            OnHPchange?.Invoke(this, -1);
        }
        if (currentHP == 0) die(.5f);
    }

    public void heal()
    {
        currentHP++;
        OnHPchange?.Invoke(this, 1);
    }

    public void die(float time)
    {
        speedModifier = 0;
        Invoke("lose", .5f);
    }

    //не очень красиво так вызывать окна, но времени больше нет
    void lose()
    {
        ui.showLoseWindow();
    }

    public event EventHandler<int> OnHPchange;

    void startInvulnerability(float time)
    {
        invulnerable = true;
        //включаем игнорирование коллизий с дамажащими объектами
        gameObject.layer = 9;
        //включаем мерцание спрайта
        animator.SetLayerWeight(1, 1);
        Invoke("stopInvulnerability", time);
    }

    void stopInvulnerability()
    {
        invulnerable = false;
        //выключаем игнорирование коллизий с дамажащими объектами
        gameObject.layer = 6;
        //выключаем мерцание спрайта
        animator.SetLayerWeight(1, 0);
    }

    void restartLevel(InputAction.CallbackContext _)
    {
        SceneManager.LoadScene(0);
    }

    private void OnDestroy()
    {
        playerInput.actions["Bomb"].performed -= spawnBomb;
        playerInput.actions["Restart"].performed -= restartLevel;
    }
}

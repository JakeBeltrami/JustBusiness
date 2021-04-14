using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IHasTile
{
    public Color colour;
    public int maxHP;
    public Vector2 initialDirection;
    protected GameManager gameManager;
    protected PathFind.Tile tile; // What tile the Entity is on
    protected Vector2 direction;
    protected bool aiming;
    protected Aiming aimScript;
    protected Animator animator;
    protected PathFind.Grid grid;
    protected Blood blood;
    protected EntityInput myInput;
    protected int health;
    protected MeleeAttack melee;
    protected SpriteRenderer shadow;


    public Vector2 Direction { get { return direction; } }
    public PathFind.Tile Tile { get { return tile; } set { tile = value; } }
    public bool Aiming { get { return aiming; } }
    public Animator Animator { get { return animator; } }
    public EntityInput MyInput { get { return myInput; } }

    public virtual void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        aimScript = GetComponentInChildren<Aiming>();
        grid = gameManager.Grid;
        blood = gameManager.GetComponent<Blood>();
        myInput = GetComponent<EntityInput>();
        melee = GetComponent<MeleeAttack>();
        SetDirection(transform.position + (Vector3)Vector2.ClampMagnitude(initialDirection, 1f));
    }

    public virtual void Start()
    {
        // Set our tile
        tile = grid.GetTile(transform.position);
        if (tile != null)
        {
            tile.Occupied = true;
        }
        health = maxHP;
        shadow = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    public void SetTile(PathFind.Tile target)
    {
        if (tile != null)
        {
            tile.Occupied = false;
        }
        tile = target;
        tile.Occupied = true;
    }

    // public void Update()
    // {
    //     //Debug.Log(name + " tile is: " + tile.Center);
    // }

    /// <summary>
    /// Set the direction for the entity to face in
    /// </summary>
    /// <param name="target">Where to look</param>
    public virtual void SetDirection(Vector3 target)
    {
        direction = (target - transform.position).normalized;
        // Debug.Log(name + " direction is: " + direction);
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }

    public abstract IEnumerator TakeTurn();

    public virtual void TakeDamage(int amount, Vector2 source, string type = "none")
    {
        // Spawn blood
        blood.SpawnBlood(GetComponent<BoxCollider2D>().bounds.center, source, colour);

        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    public abstract void Die();


}

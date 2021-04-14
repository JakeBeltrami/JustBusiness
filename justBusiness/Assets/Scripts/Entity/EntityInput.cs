using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EntityInput : MonoBehaviour
{
    protected PathFind.Grid grid;
    protected Entity self;
    protected AudioSource audioSource;
    protected MeleeAttack melee;
    protected GameManager gameManager;
    protected NewInventory inventory;
    protected LayerMask mask;
    protected GameObject target;
    protected string action;
    protected List<PathFind.Tile> path;


    public AudioClip moveSound;
    public float speed;

    public LayerMask Mask { get { return mask; } }
    public GameObject Target { get { return target; } set { target = value; } }
    public string Action { get { return action; } set { action = value; } }
    public List<PathFind.Tile> Path { get { return path; } }
    public NewInventory Inventory { get { return inventory; } }

    public virtual void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        grid = gameManager.GetComponent<PathFind.Grid>();
        self = GetComponent<Entity>();
        audioSource = GetComponent<AudioSource>();
        melee = GetComponent<MeleeAttack>();
        inventory = GetComponent<NewInventory>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            path = PathFind.Pathfinding.FindPath(grid, self.Tile, GameObject.FindGameObjectWithTag("Player").GetComponent<IHasTile>().Tile);
        }
    }

    // Jake
    // Takes a vector2 and moves the player to the corresponding grid tile
    // Also change the player's current tile to empty and the target tile to player
    public virtual IEnumerator MeleeMove(PathFind.Tile targetTile)
    {
        bool attacking = true;
        // Ratio is for smoothing
        float ratio;
        // Sound for movement
        audioSource.volume = 1;
        audioSource.PlayOneShot(moveSound);

        // Set target tile to occupied in advance
        targetTile.Occupied = true;

        // Face destination
        self.SetDirection(targetTile.Center);

        // Distance between current and target position
        float totalDist = Vector2.Distance(self.Tile.Center, targetTile.Center);
        Vector2 tarPos = targetTile.Center;

        // While moving
        while ((Vector2)transform.position != tarPos)
        {
            float distance = Vector2.Distance(transform.position, tarPos);

            if (distance / totalDist > 0.8)
            {
                // Ease in
                ratio = distance / totalDist + 0.1f;
            }
            else if (distance / totalDist > 0.3)
            {
                // Max speed
                ratio = 1;
            }
            else
            {
                // Ease out
                ratio = distance / totalDist + 0.3f;
            }
            // If we're in melee range, attack Player
            if (distance <= melee.Range && attacking)
            {
                melee.Attack(tarPos, mask);
                attacking = false;
            }

            transform.position = Vector2.MoveTowards(transform.position, tarPos, ratio * speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        self.SetTile(targetTile);
    }

    public virtual IEnumerator Move(PathFind.Tile targetTile)
    {
        // Ratio is for smoothing
        float ratio;
        // Sound for movement
        audioSource.volume = 1;
        audioSource.PlayOneShot(moveSound);

        // Set target tile to occupied in advance
        targetTile.Occupied = true;

        // Face destination
        self.SetDirection(targetTile.Center);

        // Distance between current and target position
        float totalDist = Vector2.Distance(self.Tile.Center, targetTile.Center);
        // Set tile to new destination
        //        self.SetTile(targetTile);
        // Offset target (Apparently not needed anymore???)
        Vector2 tarPos = targetTile.Center;
        //Debug.Log("Target tile is: " + targetTile.Center);
        // tarPos.y += GetComponent<Collider2D>().bounds.extents.y;

        // While moving
        while ((Vector2)transform.position != tarPos)
        {
            float distance = Vector2.Distance(transform.position, tarPos);

            if (distance / totalDist > 0.8)
            {
                // Ease in
                ratio = distance / totalDist + 0.1f;
            }
            else if (distance / totalDist > 0.3)
            {
                // Max speed
                ratio = 1;
            }
            else
            {
                // Ease out
                ratio = distance / totalDist + 0.3f;
            }

            transform.position = Vector2.MoveTowards(transform.position, tarPos, ratio * speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        self.SetTile(targetTile);
    }

    public virtual IEnumerator MoveToTile(PathFind.Tile targetTile)
    {
        // Calculate the path from self to target
        path = PathFind.Pathfinding.FindPath(grid, self.Tile, targetTile);
        if (path != null)
        {
            // If we can move to that Tile
            if (!path[0].Occupied || path[0] == targetTile)
            {
                yield return StartCoroutine(Move(path[0]));
            }
            else
            {
                yield return null;
            }
        }
    }

    public virtual void Fire(GameObject target)
    {
        self.SetDirection(Target.transform.position);
        Shoot(Target.GetComponent<BoxCollider2D>().bounds.center, Mask);
    }

    // Overwrite in Player
    public virtual IEnumerator Roll(PathFind.Tile targetTile)
    {
        yield return null;
    }

    public void Shoot(Vector2 target, LayerMask mask)
    {
        // Aim - Start animation and set bools that way?
        // self.Animator.SetBool("aiming", true); // Need to disable at some point
        // Add delay, so we can actually see us aiming? Maybe aim when selecting POI with a gun?
        // Shoot at enemy
        inventory.Weapon.Attack(target, mask);
    }
}

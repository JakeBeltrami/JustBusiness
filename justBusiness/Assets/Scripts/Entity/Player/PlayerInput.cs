using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
======================================================================

	Purpose:
		Core functions for player's attack

	Parameters:
		None

	Dependencies:
		Player.cs
		NewInventory.cs

	Author:
		Kshitij Choudhary

	Changelog:

		16-04-2019	Updated dash and attack	
		24-04-2019  Made changes to fix camerashake
        05-05-2019  Changed to support different inputs
        07-05-2019  Updated for new weapon system
        09-09-2019  Added support for detecting cover
        24-09-2019  Validator expanded to prevent shooting from behind cover
        26-09-2019  Removed some redundant code related to CoverSystem
        27-09-2019  Added cover animation check to Move
        29-09-2019  Added functionality for entering/leaving cover at the same node
        01-09-2019  SeekCover relocated so that CoverNode detection is delayed until actions are complete

	Notes:
		

======================================================================
*/

public class PlayerInput : EntityInput
{
    private Player player;
    private POI poi;
    private TurnManager turnManager;
    private CoverSystem cover;
    private bool kick;
    public float dashRange;
    public Vector2 startingTile;

    public bool Kick { get { return kick; } }
    public CoverSystem Cover { get { return cover; } }

    public override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
        poi = gameManager.GetComponent<POI>();
        turnManager = gameManager.GetComponent<TurnManager>();
        cover = GetComponent<CoverSystem>();
        mask = LayerMask.GetMask("Enemy");
        kick = false;
    }

    public void Update()
    {
        if (turnManager.GameState == TurnManager.State.Frozen && !gameManager.Planning && !gameManager.LevelWon)
        {
            GameObject target = poi.CheckPOIInteraction(dashRange);
            //Debug.Log(Action);
            // Movement/Melee testing
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetButtonDown("A"))
            {
                //GameObject target = poi.CheckPOIInteraction(dashRange);
                // If we hit a POI
                if (target != null)
                {
                    float distance = Vector2.Distance(transform.position, target.transform.position);
                    if (distance <= dashRange && target.GetComponentInChildren<POIType>().Melee)
                    {
                        // If an Enemy
                        if (target.CompareTag("Enemy"))
                        {
                            // If not close enough to melee
                            if (distance > melee.Range)
                            {
                                // Move and melee
                                Action = "MeleeMove";
                            }
                            else
                            {
                                // Melee Attack
                                Action = "Melee";
                            }
                        }
                        else
                        {
                            // If target is just a space to move to
                            if (target.GetComponentInParent<SetTile>().NavigationOnly)
                            {
                                Action = "Move";
                            }
                            else
                            {
                                Action = "Roll";
                            }
                        }

                        Target = target;

                        gameManager.GetComponent<TurnManager>().PlayerActed = true;

                        /*if (gameManager.GetComponent<TurnManager>().GameState == TurnManager.State.Frozen)
                        {
                            gameManager.GetComponent<TurnManager>().PlayerActed = true;
                        }*/
                    }
                    else if (inventory.weapon != null && inventory.weapon.Ammo > 0)
                    {
                        // Debug.Log("Shooting");
                        target = poi.CheckShootTarget();
                        // If we hit a POI
                        if (target != null && (Vector2.Distance(transform.position, target.transform.position) < inventory.weapon.Range))
                        {
                            // Condition expanded by W.T. (24-09-2019)						
                            // Prevents player from shooting while still behind cover
                            //Condition removed by K.C. (04-10-2019)
                            if (target.CompareTag("Enemy"))
                            {
                                // Set Animation
                                self.Animator.SetBool("aiming", true);
                                Action = "Shoot";

                                Target = target;

                                gameManager.GetComponent<TurnManager>().PlayerActed = true;
                            }
                            else
                            {
                                Debug.Log("Nothing to shoot");
                            }
                        }
                    }
                }
            }
            /*else if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetButtonDown("RightTrigger"))
            {
                // TEMPORARY, you can shoot enemies anywhere on the screen, change later
                target = poi.CheckShootTarget();

                //GameObject target = poi.CheckPOIInteraction(dashRange);
                // If we hit a POI
                if (target != null)
                {
                    if (inventory.weapon != null && inventory.weapon.Ammo > 0)
                    {
                        // Condition expanded by W.T. (24-09-2019)
                        // Prevents player from shooting while still behind cover
                        if (target.CompareTag("Enemy") && !player.IsInCover)
                        {
                            // Set Animation
                            self.Animator.SetBool("aiming", true);
                            Action = "Shoot";

                            Target = target;

                            gameManager.GetComponent<TurnManager>().PlayerActed = true;
                        }
                        else
                        {
                            Debug.Log("Nothing to shoot or still in cover and can't shoot.");
                        }
                    }
                }
            }*/

            //if (Input.GetKeyDown(KeyCode.W))
            //{
                //StartCoroutine(gameManager.NextLevel());
            //}
        }
    }
    public override IEnumerator Move(PathFind.Tile targetTile)
    {
        // Added by W.T. (01-10-2019)
        // Get out of cover first
        cover.LeaveCover();
        self.Animator.SetBool("moving", true);
        yield return StartCoroutine(base.Move(targetTile));
        self.Animator.SetBool("moving", false);
        cover.SeekCover(targetTile.Center);
        // Added by W.T. (29-09-2019)
        // Cover animation only allowed if the player is in cover
        if (player.IsInCover)
        {
            cover.EnterCover();
        }
        DelayTurn(0);
    }

    public override IEnumerator MeleeMove(PathFind.Tile targetTile)
    {
        cover.LeaveCover();
        self.Animator.SetBool("dashing", true);
        yield return StartCoroutine(base.MeleeMove(targetTile));
        self.Animator.SetBool("dashing", false);
        cover.SeekCover(targetTile.Center);
        // Added by W.T. (29-09-2019)
        // Cover animation only allowed if the player is in cover
        if (player.IsInCover)
        {
            cover.EnterCover();
        }
        DelayTurn(0.2f);
    }

    public override IEnumerator Roll(PathFind.Tile targetTile)
    {
        cover.LeaveCover();
        player.Animator.SetBool("roll", true);
        yield return StartCoroutine(base.Move(targetTile));
        player.Animator.SetBool("roll", false);
        cover.SeekCover(targetTile.Center);
        if (player.IsInCover)
        {
            cover.EnterCover();
        }
        DelayTurn(0);
    }

    public IEnumerator MoveToDoor(PathFind.Tile targetTile)
    {
        yield return new WaitForSeconds(1);
        // Calculate the path from self to target
        path = PathFind.Pathfinding.FindPath(grid, self.Tile, targetTile);
        if (path != null)
        {
            // Loop through path to door
            foreach (PathFind.Tile tile in path)
            {
                yield return StartCoroutine(Move(tile));
                // Wait longer?
                yield return new WaitForEndOfFrame();
                // yield return new WaitForSeconds(2);
            }
            // Start kicking animation
            Target = null;
            self.SetDirection(transform.position + Vector3.up);
            self.Animator.SetBool("kick", true);
        }
    }

    public override void Fire(GameObject target)
    {
        cover.LeaveCover();
        base.Fire(target);
        // Alert all enemies
        foreach (GameObject e in gameManager.Enemies)
        {
            e.GetComponent<Enemy>().Alert(self.Tile.Center);
        }
    }

    public IEnumerator MoveToEnd(PathFind.Tile targetTile)
    {
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(Move(targetTile));
    }

    public void KickDoor()
    {
        kick = true;
        self.Animator.SetBool("kick", false);
    }

    public void DelayTurn(float seconds = 0.5f)
    {
        if (seconds > 0f)
        {
            StartCoroutine(DelayTurnEnd(seconds));
        }
        else
        {
            gameManager.TurnManager.GameState = TurnManager.State.EnemyTurn;
        }
    }

    public IEnumerator DelayTurnEnd(float seconds = 1f)
    {
        yield return new WaitForSeconds(seconds);
        gameManager.TurnManager.GameState = TurnManager.State.EnemyTurn;
    }

    public IEnumerator WalkIn()
    {
        if (startingTile != Vector2.zero)
        {
            // Wait before moving
            yield return new WaitForSeconds(0.5f);
            self.Animator.SetBool("moving", true);
            // Ratio is for smoothing
            float ratio;
            float speed = 5f;
            // Sound for movement
            audioSource.volume = 1;
            audioSource.PlayOneShot(moveSound);

            // Face destination
            self.SetDirection(startingTile);

            // Distance between current and target position
            float totalDist = Vector2.Distance(transform.position, startingTile);
            // Set tile to new destination
            Vector2 tarPos = startingTile;

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
            self.Animator.SetBool("moving", false);
            self.SetTile(grid.GetTile(startingTile));
        }
        // Activate clapper
        gameManager.Clapper.enabled = true;
        // Disable component
        enabled = false;
    }
}
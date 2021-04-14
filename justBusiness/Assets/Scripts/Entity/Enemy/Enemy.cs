using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
======================================================================

    Purpose:
        Core functions for Enemies

    Parameters:
        None

    Dependencies:
        Player.cs
        CameraShake.cs

    Author:
        Phil

    Changelog:
        29-04-2019  Made changes for shooting
        30-04-2019  Drop weapon on death
        04-05-2019  Temporary adjustment for hiding URE warnings (unassigned weapon2)
        05-05-2019  Added extra condition so that players don't get stunned if they're dashing
        07-05-2019  Updated with latest weapon system
        11-05-2019  Added line-of-sight detection
        12-05-2019  Added extra properties related to boolean operations

    Notes:
        Will need a variable for melee range to determine if can attack or not?

======================================================================
*/

public class Enemy : Entity
{
    // Enumerators
    public enum State
    {
        Patrol,
        Alert,
        Agro,
        Aiming
    }

    protected GameObject player;
    protected SoundManager sound;
    protected State myState;
    protected FieldOfView fov;
    protected Patrol patrol;
    protected NewInventory inventory;
    protected SlowDown slowDown;
    protected ShakeTest cameraShake;
    protected PathFind.Tile alertTile;

    protected bool agro;

    public GameObject marker;
    private Animator markerAnim;

    public FieldOfView FOV { get { return fov; } }
    public State MyState { get { return myState; } set { myState = value; } }

    public override void Start()
    {
        base.Start();
        fov = GetComponentInChildren<FieldOfView>();
        player = GameObject.FindGameObjectWithTag("Player");
        sound = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        patrol = GetComponent<Patrol>();
        inventory = GetComponent<NewInventory>();
        slowDown = gameManager.GetComponent<SlowDown>();
        cameraShake = GameObject.FindGameObjectWithTag("Camera").GetComponent<ShakeTest>();
        markerAnim = marker.GetComponent<Animator>();
        agro = false;

        // Set hasGun value in Animator
        if (inventory.weapon == null)
        {
            animator.SetBool("hasGun", false);
        }

        // Initialise alt enemy
        transform.GetComponentInChildren<AltEnemy>().Init();

        // Making sure direction is set
        // SetDirection(transform.position + (Vector3)Vector2.ClampMagnitude(initialDirection, 1f));
    }

    // public void Update()
    // {
    //     Debug.Log(name + " moveY is: " + animator.GetFloat("moveY"));
    // }

    public void SetInitialDirection()
    {
        SetDirection(transform.position + (Vector3)Vector2.ClampMagnitude(initialDirection, 1f));
    }

    public void SetInitialState()
    {
        fov = GetComponentInChildren<FieldOfView>();
        fov.FindVisibleTargets();
        myState = State.Patrol;
        if (fov.PlayerSpotted)
        {
            Agro(player.transform.position);
        }
    }

    public override IEnumerator TakeTurn()
    {
        // Debug.Log(name + "state is: " + myState);
        switch (myState)
        {
            case State.Patrol:
                markerAnim.Play("None", 0);
                // marker.GetComponent<SpriteRenderer>().sprite = null;
                // if (fov.PlayerSpotted)
                // {
                //     Alert(player.transform.position);
                // }
                if (patrol.patrolPoints.Length > 0)
                {
                    patrol.NextPatrol();
                }
                gameManager.GetComponent<TurnManager>().EnemyCount++;
                break;
            // Alert should just make them turn to face sound
            case State.Alert:
                // If we see Player, become agro
                if (fov.PlayerSpotted)
                {
                    AgroFromAlert(player.transform.position);
                }
                else
                {
                    // Pathfinding to source of sound
                    // marker.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("EnemyMarker/Active_Notice");
                    StartCoroutine(myInput.MoveToTile(alertTile));
                }
                gameManager.GetComponent<TurnManager>().EnemyCount++;
                break;

            case State.Agro:
                myInput.Target = player;
                PathFind.Tile playerTile = player.GetComponent<IHasTile>().Tile;
                // marker.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("EnemyMarker/Active_Notice");
                // If within melee range, opt to melee
                float distance = Vector2.Distance(Tile.Center, playerTile.Center);
                if (distance <= melee.Range)
                {
                    Debug.Log("melee");
                    markerAnim.Play("Melee", 0);
                    // marker.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("EnemyMarker/Active_Fist");
                    melee.Attack(player.GetComponent<BoxCollider2D>().bounds.center, myInput.Mask);
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    // Aim
                    if (inventory.weapon != null)
                    {
                        if (inventory.weapon.Ammo > 0)
                        {
                            markerAnim.Play("Aiming", 0);
                            // marker.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("EnemyMarker/Active_Gun");
                            myState = State.Aiming;
                            animator.SetBool("aiming", true);
                            SetDirection(player.transform.position);
                            // aimScript.Aim(player.transform.position);
                            gameManager.GetComponent<TurnManager>().EnemyCount++;
                            break;
                        }
                    }
                    else
                    {
                        // Pathfind to player
                        // Debug.Log("Pathfinding");
                        markerAnim.Play("Active", 0);
                        yield return (StartCoroutine(myInput.MoveToTile(playerTile)));
                    }
                }
                gameManager.GetComponent<TurnManager>().EnemyCount++;
                break;

            case State.Aiming:
                myInput.Shoot(myInput.Target.GetComponent<BoxCollider2D>().bounds.center, myInput.Mask);
                markerAnim.Play("Reload", 0);
                // marker.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("EnemyMarker/Active_Notice");
                myState = State.Agro;
                break;
        }

        // gameManager.GetComponent<TurnManager>().EnemyCount++;
    }

    public override void Die()
    {
        markerAnim.Play("None", 0);
        fov.enabled = false;
        // Remove self from enemies list
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().Enemies.Remove(gameObject);

        foreach (GameObject e in gameManager.Enemies)
        {
            // StartCoroutine(e.GetComponent<Enemy>().AlertWithDelay(GetComponent<Entity>().Tile.Center, 1f));
            e.GetComponent<Enemy>().Alert(GetComponent<Entity>().Tile.Center);
        }

        slowDown.SlowTime();
        StartCoroutine(cameraShake.ShakeForSeconds());

        // Death animation
        animator.SetBool("dead", true);
        // Move them slightly up
        transform.position += Vector3.up * 0.1f;

        sound.PlayEnemyDeath();

        // Disable components
        GetComponent<BoxCollider2D>().enabled = false;
        shadow.enabled = false;
        enabled = false;
        // Enable POI
        // transform.Find("POI").gameObject.SetActive(true);
        // Delete POI
        Destroy(transform.Find("POI").gameObject);
        myInput.StopAllCoroutines();
        myInput.enabled = false;

        enabled = false;
        Tile.Occupied = false;
    }

    public override void SetDirection(Vector3 target)
    {
        base.SetDirection(target);
        if (fov != null)
        {
            fov.FaceDirection();
        }
    }

    public void Alert(Vector2 target)
    {
        if (myState == State.Patrol)
        {
            myState = State.Alert;
            SetDirection(target);
            alertTile = gameManager.Grid.GetTile(target);
            markerAnim.Play("Alert", 0);
            // marker.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("EnemyMarker/Alert");
        }
    }

    public IEnumerator AlertWithDelay(Vector2 target, float delay)
    {
        yield return new WaitForSeconds(delay);
        Alert(target);
    }

    public void Agro(Vector2 target)
    {
        if (MyState == State.Patrol)
        {
            myState = State.Agro;
            SetDirection(target);
            markerAnim.Play("Agro", 0);
            // marker.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("EnemyMarker/Active_Notice");
        }
    }

    public void AgroFromAlert(Vector2 target)
    {
        if (MyState == State.Alert)
        {
            myState = State.Agro;
            SetDirection(target);
            markerAnim.Play("Agro", 0);
        }
    }

}

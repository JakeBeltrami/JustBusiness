using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class EnemyInput : EntityInput
{

    public override void Start()
    {
        base.Start();
        mask = LayerMask.GetMask("Player");
    }

    // NOTE: Need some way to set melee animation
    public override IEnumerator Move(PathFind.Tile targetTile)
    {
        self.Animator.SetBool("moving", true);
        yield return StartCoroutine(base.Move(targetTile));
        self.Animator.SetBool("moving", false);
        self.Animator.SetBool("attacking", false);
    }

    public override IEnumerator MeleeMove(PathFind.Tile targetTile)
    {
        // Manual timing for attack
        float meleeRange = melee.Range + 1f;
        bool attacking = true;
        bool startedAttacking = false;
        // Ratio is for smoothing
        float ratio;
        // Sound for movement
        audioSource.volume = 1;
        audioSource.PlayOneShot(moveSound);

        // Face destination
        self.SetDirection(targetTile.Center);

        // Distance between current and target position
        float totalDist = Vector2.Distance(self.Tile.Center, targetTile.Center);
        Vector2 tarPos = targetTile.Center;

        // While moving
        while ((Vector2)transform.position != tarPos)
        {
            float distance = Vector2.Distance(transform.position, tarPos);
            // Debug.Log(distance);

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
            if (distance <= meleeRange && !startedAttacking)
            {
                self.Animator.SetBool("attacking", true);
                startedAttacking = true;
            }
            // If we're in melee range, attack Player
            if (distance <= melee.Range && attacking)
            {
                melee.Attack(gameManager.Player.Tile.Center, mask);
                attacking = false;
            }

            transform.position = Vector2.MoveTowards(transform.position, tarPos, ratio * speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        self.SetTile(targetTile);
    }

    public override IEnumerator MoveToTile(PathFind.Tile targetTile)
    {
        // Calculate the path from self to target
        path = PathFind.Pathfinding.FindPath(grid, self.Tile, targetTile);
        if (path != null)
        {
            // Debug.Log(name + " is moving to: " + path[0].Center);
            // Debug.Log(name + " first tile occupied?: " + path[0].Occupied);
            // If we can move to that Tile
            if (!path[0].Occupied || path[0] == gameManager.Player.Tile)
            {
                // If within melee distance and is the player tile
                float distance = Vector2.Distance(path[0].Center, targetTile.Center);
                //Debug.Log(distance);
                //Debug.Log(gameManager.Player.Tile.Center);
                //Debug.Log(targetTile.Center);
                if ((distance <= GetComponent<MeleeAttack>().Range) && (targetTile == gameManager.Player.Tile))
                {
                    Debug.Log("Move and Melee");
                    yield return StartCoroutine(MeleeMove(path[0]));
                }
                else
                {
                    yield return StartCoroutine(Move(path[0]));
                }
            }
            else
            {
                yield return null;
            }
        }

        if (GetComponentInChildren<FieldOfView>().PlayerSpotted && GetComponent<Enemy>().MyState == Enemy.State.Alert)
        {
            GetComponent<Enemy>().marker.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("EnemyMarker/Alert");
        }
        // else
        // {
        //     Debug.Log("Path is null");
        // }
    }
}

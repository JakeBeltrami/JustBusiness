using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class EnemyHeavyInput : EnemyInput
{
    public override IEnumerator MeleeMove(PathFind.Tile targetTile)
    {
        // Manual timing for attack
        float meleeRange = melee.Range + 2f;
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
            if (distance <= meleeRange && attacking)
            {
                melee.Attack(gameManager.Player.Tile.Center, mask);
                attacking = false;
            }

            transform.position = Vector2.MoveTowards(transform.position, tarPos, ratio * speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        self.SetTile(targetTile);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : BaseAttack
{
    public float radius; // How big the overlap circle is
    private Vector2 detectionPoint;
    private GameManager gameManager;

    private void Start()
    {
        // Set default values here (Overwrites inspector)
        /*damage = 1;
        range = 1f;
        radius = 0.2f;*/
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    /// <summary>
    /// Performs a melee attack on the target
    /// </summary>
    /// <param name="target"></param>
    /// <param name="layerMask"></param>
    public override void Attack(Vector2 target, LayerMask layerMask)
    {
        // Perform animation?
        // Play sound?
        // if (tag == "Enemy")
        // {
        //     GetComponent<Entity>().Animator.SetBool("attacking", true);
        // }

        // Perform attack
        detectionPoint = target;
        Collider2D hit = Physics2D.OverlapCircle(detectionPoint, radius, layerMask); // Radius might need changing

        // If we hit something
        if (hit != null)
        {
            hit.GetComponent<Entity>().TakeDamage(damage, GetComponent<BoxCollider2D>().bounds.center, "melee");
            // Debug.Log("Hit: " + hit.gameObject.name);
        }
    }

    // Draw the attack (Testing)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint, radius);
    }
}

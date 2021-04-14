using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltEnemy : MonoBehaviour
{
    private Animator animator;
    private Enemy self;

    public void Init()
    {
        animator = GetComponent<Animator>();
        self = GetComponentInParent<Enemy>();
        FlipDirection(-self.initialDirection);
        animator.SetBool("hasGun", self.Animator.GetBool("hasGun"));
        Debug.Log(animator.GetBool("outlined"));
        animator.SetBool("outlined", false);
        Debug.Log(animator.GetBool("outlined"));
    }

    public void FlipDirection(Vector3 direction)
    {
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }
}

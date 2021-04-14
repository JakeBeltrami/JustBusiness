using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTest : MonoBehaviour
{
    private Animator animator;
    private Aiming aimScript;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("aiming", true);
        aimScript = GetComponentInChildren<Aiming>();
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition - transform.position).normalized;
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
        // Track mouse for testing
        aimScript.Aim(mousePosition);
        // Test shooting
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GetComponent<KnifeAttack>().Attack(mousePosition, 0);
        }
    }
}

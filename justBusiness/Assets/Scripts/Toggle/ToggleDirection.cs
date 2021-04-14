using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class ToggleDirection : Toggleable
{
    private Enemy self;
    private AltEnemy alt;

    public override void Start()
    {
        base.Start();
        self = GetComponent<Enemy>();
        alt = self.GetComponentInChildren<AltEnemy>();
    }

    public override void Toggle()
    {
        self.SetDirection(self.transform.position - (Vector3)self.Direction);
        alt.FlipDirection(-self.Direction);
    }
}

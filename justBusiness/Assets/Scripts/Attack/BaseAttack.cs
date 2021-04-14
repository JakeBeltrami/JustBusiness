using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttack : MonoBehaviour
{
    public int damage; // Amount of damage
    public float range; // Distance of attack
    public int ammo;

    public float Range { get { return range; } }
    public int Ammo {  get { return ammo; } set { ammo = value; } }

    public abstract void Attack(Vector2 target, LayerMask layerMask);
}

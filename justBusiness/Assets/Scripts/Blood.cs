using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    public GameObject bloodParticle;
    public GameObject bloodPool;
    public GameObject bloodPoolSprite;
    public GameObject bloodParticleSmall;

    public void SpawnBlood(Vector3 source, Vector3 impact, Color colour, bool small = false)
    {
        // Spawn and set the colour of the blood spray
        Vector3 direction = (source - impact).normalized;
        GameObject blood;
        float angle = AngleFromDir(direction) + 60f; // Calculate offset
        if (small)
        {
            blood = Instantiate(bloodParticleSmall, source, Quaternion.Euler(0, 0, angle));
        }
        else
        {
            blood = Instantiate(bloodParticle, source, Quaternion.Euler(0, 0, angle));
        }
        // blood.transform.right = DirFromAngle(angle); // Align blood spray in direction
        ParticleSystem.MainModule main = blood.GetComponent<ParticleSystem>().main;
        main.startColor = new ParticleSystem.MinMaxGradient(colour);

        if (!small)
        {
            // Spawn and set the colour of the blood pool
            GameObject blood_pool = Instantiate(bloodPool, source, Quaternion.identity);
            main = blood_pool.GetComponent<ParticleSystem>().main;
            main.startColor = new ParticleSystem.MinMaxGradient(colour);

            // Spawn and set the colour of the sprite
            // Hardcoded offset for now
            GameObject blood_pool_sprite = Instantiate(bloodPoolSprite, source - (Vector3.up * 0.9f), Quaternion.Euler(0, 0, angle));
            blood_pool_sprite.GetComponent<SpriteRenderer>().color = colour;
            blood_pool_sprite.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    // Copied from FieldOfView.cs, should this change?
    public float AngleFromDir(Vector3 direction)
    {
        return Vector3.SignedAngle(Vector3.up, direction, Vector3.forward);
    }

    public Vector3 DirFromAngle(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }
}

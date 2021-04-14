using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ToggleWeapon : Toggleable
{
    public GameObject altObj;
    public Sprite originalSprite;
    private Transform altPos; // Assign from empty GameObject in scene?
    private Transform origPos;
    private Queue<Vector2> positions;
    private bool moved;
    private SetTile setTile;

    public override void Start()
    {
        base.Start();
        moved = false;
        setTile = GetComponent<SetTile>();
        // Assign current position
        origPos = transform;
        // Assign alt position
        altPos = altObj.transform;
        // Could also delete POI from alt
        altObj.transform.GetChild(0).gameObject.SetActive(false);
        positions = new Queue<Vector2>();
        positions.Enqueue(altPos.position);
        positions.Enqueue(origPos.position);
    }

    /// <summary>
    /// Move the object to the next position
    /// </summary>
    public override void Toggle() // NOTE: Will only work if Time scale is 1
    {
        moved = !moved;
        Vector2 newPos1 = positions.Dequeue(); // Get the next position
        transform.position = newPos1;
        Vector2 newPos2 = positions.Dequeue();
        altObj.transform.position = newPos2;
        // Register positions in reverse order
        positions.Enqueue(newPos2);
        positions.Enqueue(newPos1);
    }

    public override void Remove()
    {
        base.Remove();
        // If not the active weapon
        if (moved)
        {
            setTile.Set(altObj.GetComponent<SetTile>().pos);
        }
        GetComponent<OffsetWeapon>().Offset();
        GetComponent<SpriteRenderer>().sprite = originalSprite;
        Destroy(altObj);
    }
}

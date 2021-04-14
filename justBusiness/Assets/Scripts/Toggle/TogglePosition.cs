using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePosition : Toggleable
{
    public GameObject altObj;
    public Sprite originalSprite;
    private Transform altPos; // Assign from empty GameObject in scene?
    private Transform origPos;
    private Queue<Vector2> positions;

    public override void Start()
    {
        base.Start();
        // Assign current position
        origPos = transform;
        // Assign alt position
        altPos = altObj.transform;
        positions = new Queue<Vector2>();
        positions.Enqueue(altPos.position);
        positions.Enqueue(origPos.position);
    }

    /// <summary>
    /// Move the object to the next position
    /// </summary>
    public override void Toggle() // NOTE: Will only work if Time scale is 1
    {
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
        if (GetComponentInChildren<SetTile>() != null)
        {
            // Debug.Log("Setting");
            GetComponentInChildren<SetTile>().Set();
        }
        Destroy(altObj);
        GetComponent<SpriteRenderer>().sprite = originalSprite;
        base.Remove();
    }
}

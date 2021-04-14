using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetWeapon : MonoBehaviour
{
    private BoxCollider2D col;
    private bool toggle;

    private void Start()
    {
        toggle = GetComponent<Toggleable>() != null ? true : false;
        if (!toggle)
        {
            Offset();
        }
    }

    public void Offset()
    {
        col = GetComponent<BoxCollider2D>();
        col.size = new Vector2(0.1f, 0.1f);
        col.offset = GetComponent<SetTile>().pos - (Vector2)transform.position;
    }
}

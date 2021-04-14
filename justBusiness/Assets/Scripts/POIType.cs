using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POIType : MonoBehaviour
{
    private bool melee;
    private bool selectable;
    private bool enemy;
    public bool Melee { get { return melee; } set { melee = value; } }
    public bool Selectable { get { return selectable; } set { selectable = value; } }
    private Dictionary<string, Texture2D> cursorMap;

    public virtual void Start()
    {
        cursorMap = new Dictionary<string, Texture2D>
        {
            { "melee" , Resources.Load<Texture2D>("UI/Melee Cursor") },
            { "shoot" , Resources.Load<Texture2D>("UI/Shoot Cursor") },
            { "hand" , Resources.Load<Texture2D>("UI/Hand Cursor") },
        };
        enemy = GetComponentInParent<Enemy>() != null ? true : false;
    }

    public void OnMouseEnter()
    {
        // Debug.Log("Hovering over POI");
        Texture2D cursor;
        if (selectable)
        {
            // If an enemy and in range
            if (enemy && melee)
            {
                cursor = cursorMap["melee"];
                Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.Auto);
            }
            // In range but not an enemy
            else if (melee)
            {
                cursor = cursorMap["hand"];
                Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.Auto);
            }
            // A target to shoot
            else
            {
                cursor = cursorMap["shoot"];
                Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.Auto);
            }
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    public void OnMouseOver()
    {
        // Debug.Log("Hovering over POI");
        Texture2D cursor;
        if (selectable)
        {
            // If an enemy and in range
            if (enemy && melee)
            {
                cursor = cursorMap["melee"];
                Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.Auto);
            }
            // In range but not an enemy
            else if (melee)
            {
                cursor = cursorMap["hand"];
                Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.Auto);
            }
            // A target to shoot
            else
            {
                cursor = cursorMap["shoot"];
                Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.Auto);
            }
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    public void OnMouseExit()
    {
        Debug.Log("Apparently exiting POI");
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}

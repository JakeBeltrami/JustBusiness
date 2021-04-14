using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Toggleable : MonoBehaviour
{
    protected Texture2D cursor;

    public virtual void Start()
    {
        cursor = Resources.Load<Texture2D>("UI/Hand Cursor");
    }

    public abstract void Toggle();

    public virtual void Remove()
    {
        Destroy(this);
    }

    public void OnMouseOver()
    {
        Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.Auto);
    }

    public void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void OnDestroy()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}

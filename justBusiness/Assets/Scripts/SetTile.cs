using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTile : MonoBehaviour, IHasTile
{
    private PathFind.Tile tile;
    public Vector2 pos;
    public bool usePosition;
    public bool navigationOnly;
    public PathFind.Tile Tile { get { return tile; } set { tile = value; } }
    public bool NavigationOnly { get { return navigationOnly; } }

    public void Awake()
    {
        if (usePosition)
        {
            pos = transform.position;
        }
        Set(pos);
        // pos = this.transform.position;
    }

    public void Set(Vector2 position)
    {
        if (pos != position)
        {
            pos = position;
        }
        tile = GameObject.FindGameObjectWithTag("GameController").GetComponent<PathFind.Grid>().GetTile(position);
    }

    public void Set()
    {
        if (pos != (Vector2)transform.position)
        {
            pos = (Vector2)transform.position;
        }
        tile = GameObject.FindGameObjectWithTag("GameController").GetComponent<PathFind.Grid>().GetTile(pos);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(pos, Vector3.one * 0.5f);
    }
}

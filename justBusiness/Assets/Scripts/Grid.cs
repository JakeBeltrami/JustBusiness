using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    // Jake
    // The list of tiles contained within the grid.
    private List<Tile> tiles;

    // Jake
    // How many tiles wide and tall the grid is.
    [SerializeField]
    private int xTiles;
    [SerializeField]
    private int yTiles;

    // Jake
    // Properties for values.
    public int XTiles { get { return xTiles; } }
    public int YTiles { get { return yTiles; } }

    // Jake
    // In the Awake function we set up the grid by creating all of the tiles and putting them in the list.
    // Set to Awake so the grid is always available
    void Awake()
    {
        tiles = new List<Tile>();
        int i = 0;
        while (i < xTiles * yTiles)
        {
            Tile tile = new Tile(xTiles, i);
            tiles.Add(tile);
            i++;
        }
    }

    // Jake
    // The GetTile function takes in an x and y integer which represents the location of the desired tile, 
    // it then converts those coordinates into the correct list index and returns the tile.
    public Tile GetTile(Vector2 vec)
    {
        // Needs to round up X and Y first
        int x = Mathf.CeilToInt(vec.x);
        int y = Mathf.CeilToInt(vec.y);

        // Calculate index
        int targetTile = x - 1 + (y - 1) * xTiles;

        // If valid
        if (targetTile >= 0 && targetTile <= tiles.Count)
        {
            return tiles[targetTile];
        }
        else
        {
            return null;
        }
    }

    // Jake
    // Check all the tiles in a radius around the player, return a list of tiles that have either enemies or items on them.
    public List<Tile> GetTiles(Vector2 tilePos, int radius)
    {
        List<Tile> availTiles = new List<Tile>();
        // Get a list of positions to check
        List<Vector2> addVector = GetVectors(radius);

        foreach (Vector2 v in addVector)
        {
            Tile checkTile = GetTile(tilePos + v);
            // If a tile and is an enemy or item (Change to get IHasTiles?)
            if ((checkTile != null) && (checkTile.TType == Tile.tileType.enemy || checkTile.TType == Tile.tileType.item))
            {
                availTiles.Add(checkTile);
            }
        }

        return availTiles;
    }

    // Jake
    // Given a radius r, find all the tile vectors that need to be checked around the player.
    public List<Vector2> GetVectors(int r)
    {
        List<Vector2> values = new List<Vector2>();

        for (int i = r; i >= -r; i--)
        {
            int checks = 2 * (r - Mathf.Abs(i)) + 1;

            for (int j = -checks / 2; j <= checks / 2; j++)
            {
                values.Add(new Vector2(j, i));
            }
        }

        return values;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    foreach (Tile t in tiles)
    //    {
    //        Gizmos.DrawWireCube(t.Center, Vector2.one * 0.5f);
    //    }
    //}
}

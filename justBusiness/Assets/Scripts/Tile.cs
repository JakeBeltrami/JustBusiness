using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    // Jake
    // The x and y positions of the tile in the grid, starts at 1,1.
    private int xPos;
    private int yPos;

    // Jake
    // The tile enumerator that designates what occupies the tile's space.
    // Wall - Blocks movement and bullets.
    // Barrier - Blocks movement but not bullets.
    // Empty - Nothing on the tile.
    // Enemy - Enemy on the tile.
    // Player - Player on the tile.
    public enum tileType
    {
        wall,
        barrier,
        empty,
        item,
        enemy,
        player
    }

    private tileType tType;

    // Jake
    // Properties for values.
    public tileType TType { get { return tType; } set { tType = value; } }
    public Vector2 Position { get { return new Vector2(xPos, yPos); } }
    public Vector2 Center { get { return new Vector2(xPos - 0.5f, yPos - 0.5f); } }

    // Jake
    // Constructor for the tile class, we assign the tile an X and Y position and assign the tile type via a raycast.
    public Tile(int xTiles, int i)
    {
        xPos = i % xTiles + 1;
        yPos = i / xTiles + 1;

        Ray ray = new Ray(new Vector3(xPos - 0.5f, yPos - 0.5f, -1), new Vector3(0, 0, 1));
        RaycastHit2D hit;

        if (hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                TType = tileType.player;
            }
            else if (hit.collider.gameObject.tag == "Enemy")
            {
                TType = tileType.enemy;
            }
            else if (hit.collider.gameObject.tag == "Wall")
            {
                TType = tileType.wall;
            }
            else if (hit.collider.gameObject.tag == "Environment")
            {
                TType = tileType.barrier;
            }
            else if (hit.collider.gameObject.tag == "Pickup")
            {
                TType = tileType.item;
            }
            else
            {
                TType = tileType.empty;
            }
        }
        else
        {
            TType = tileType.empty;
        }
    }
}


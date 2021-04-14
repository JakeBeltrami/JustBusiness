using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: For each enemy, a path will have to be created
// Update the grid with new walkable tiles to pathfind with OR
// Remove Enemy position and onwards tiles from path
// Also possible to add script to nodes which stores the Entity on it (or just a bool)
// The grid will need a way to update walkable based on Entities
//      * Either manually or using SetTile, something else?
// If Enemies wish to move to the player, they will need to pathfind
// Will need a List<Tile> path variable and movement will be to the first Tile
// How to handle path? Find alternate route? (Maybe put useOccupied in FindPath)
public class PathfindTest : MonoBehaviour
{
    PathFind.Grid grid;
    public Transform source, target;

    void Start()
    {
        grid = GameObject.FindGameObjectWithTag("GameController").GetComponent<PathFind.Grid>();
    }

    void Update()
    {
        // Use IHasTile.Tile for start and target
        // grid.path = PathFind.Pathfinding.FindPath(grid, grid.GetTile(source.position), grid.GetTile(target.position));
    }
}

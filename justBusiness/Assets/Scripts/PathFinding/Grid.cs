/**
 * Represent a grid of nodes we can search paths on.
 * Based on code and tutorial by Sebastian Lague (https://www.youtube.com/channel/UCmtyQOKKmrMVaKuRXz02jbQ).
 *   
 * Author: Ronen Ness.
 * Since: 2016. 
 * NOTE: Heavily adjusted to suit needs (As with Tile (was Node) and Pathfinding)
*/
using System.Collections.Generic;
using UnityEngine;

namespace PathFind
{
    /// <summary>
    /// A 2D grid of nodes we use to find path.
    /// The grid mark which tiles are walkable and which are not.
    /// </summary>
    public class Grid : MonoBehaviour
    {
        private float nodeRadius;
        public Tile[,] nodes; // 2D Array to represent Grid
        public int gridSizeX, gridSizeY;
        public int range; // How far to search for nodes
        public LayerMask unwalkableMask;
        public bool visualiseGrid;
        public List<Tile> path;
        public EntityInput entity;
        public LayerMask nodeCheckMask; // Set to Environment and Node

        public Tile[,] Tiles { get { return nodes; } }

        private void Awake()
        {
            nodeRadius = 0.5f;
            CreateGrid();
        }

        /// <summary>
        /// Create a grid of nodes from the specified x and y values
        /// </summary>
        public void CreateGrid()
        {
            nodes = new Tile[gridSizeX, gridSizeY];
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector2 worldPosition = new Vector2(x + 0.5f, y + 0.5f);
                    bool walkable = !(Physics2D.OverlapCircle(worldPosition, nodeRadius / 2f, unwalkableMask));
                    nodes[x, y] = new Tile(walkable, x, y, worldPosition);
                }
            }
        }

        /// <summary>
        /// Each node updates its walkable state
        /// </summary>
        public void UpdateGrid()
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    bool walkable = !(Physics2D.OverlapCircle(nodes[x, y].position, nodeRadius / 2f, unwalkableMask));
                    nodes[x, y].Walkable = walkable;
                }
            }
        }

        /// <summary>
        /// Return a tile given a position. Ensure grid begins at origin (0,0)
        /// </summary>
        public Tile GetTile(Vector2 worldPos)
        {
            return nodes[(int)worldPos.x, (int)worldPos.y];
        }

        /// <summary>
        /// Get all the neighbors of a given tile in the grid.
        /// </summary>
        /// <param name="node">Tile to get neighbors for.</param>
        /// <returns>List of node neighbors.</returns>
        public List<Tile> GetNeighbours(Tile node, Pathfinding.DistanceType distanceType)
        {
            int x = 0, y = 0;
            List<Tile> neighbours = new List<Tile>();
            switch (distanceType)
            {
                case Pathfinding.DistanceType.Manhattan:
                    y = 0;
                    for (x = -1; x <= 1; ++x)
                    {
                        var neighbor = AddTileNeighbour(x, y, node);
                        // if (neighbor != null)
                        // yield return neighbor;
                    }

                    x = 0;
                    for (y = -1; y <= 1; ++y)
                    {
                        var neighbor = AddTileNeighbour(x, y, node);
                        // if (neighbor != null)
                        // yield return neighbor;
                    }
                    break;

                case Pathfinding.DistanceType.Euclidean:
                    // Raycast for node neighbours in cardinal directions
                    for (x = -1; x <= 1; x++)
                    {
                        for (y = -1; y <= 1; y++)
                        {
                            Vector2 direction = new Vector2(x, y);
                            RaycastHit2D nodeHit = Physics2D.Raycast(node.Center + (direction / 4f), direction, range - 0.25f, nodeCheckMask);
                            // If we hit a Node
                            if (nodeHit.collider != null && nodeHit.collider.CompareTag("Node"))
                            {
                                Tile hitTile = GetTile(nodeHit.transform.position);
                                Tile neighbor = AddTileNeighbour(hitTile.gridX, hitTile.gridY, node);
                                if (neighbor != null)
                                {
                                    neighbours.Add(neighbor);
                                }
                            }
                        }
                    }
                    break;
            }

            return neighbours;
        }

        /// <summary>
        /// Adds the node neighbour.
        /// </summary>
        /// <returns><c>true</c>, if node neighbour was added, <c>false</c> otherwise.</returns>
        Tile AddTileNeighbour(int x, int y, Tile node)
        {
            if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
            {
                return nodes[x, y];
            }

            return null;
        }

        // Visualisation
        public void OnDrawGizmos()
        {
            if (nodes != null && visualiseGrid)
            {
                // Update grid
                // UpdateGrid();
                // Visualise grid
                if (entity != null)
                {
                    path = entity.Path;
                }
                foreach (PathFind.Tile n in nodes)
                {
                    Gizmos.color = (n.Walkable) ? new Color(0, 1, 0, 0.4f) : new Color(1, 0, 0, 0.4f);
                    if (path != null)
                    {
                        if (path.Contains(n))
                        {
                            Gizmos.color = new Color(0, 1, 1, 0.4f);
                        }
                        else if (n == path[path.Count - 1])
                        {
                            Gizmos.color = new Color(1, 0, 1, 0.4f);
                        }
                    }
                    if (n.Occupied)
                    {
                        Gizmos.color = new Color(1, 0, 1, 0.4f);
                    }
                    Gizmos.DrawCube(n.position, Vector2.one * (nodeRadius * 2 - 0.1f));
                }
            }
        }
    }
}

/**
 * Provide simple path-finding algorithm with tile prices support.
 * Based on code and tutorial by Sebastian Lague (https://www.youtube.com/channel/UCmtyQOKKmrMVaKuRXz02jbQ).
 *   
 * Author: Ronen Ness.
 * Since: 2016. 
*/
using System.Collections.Generic;
using UnityEngine;

namespace PathFind
{
    /// <summary>
    /// Main class to find the best path to walk from A to B.
    /// 
    /// Usage example:
    /// Grid grid = new Grid(width, height, tiles_costs);
    /// List<Point> path = Pathfinding.FindPath(grid, from, to);
    /// </summary>
    public class Pathfinding
    {
        /// <summary>
        /// Different ways to calculate path distance.
        /// </summary>
		public enum DistanceType
        {
            /// <summary>
            /// The "ordinary" straight-line distance between two points.
            /// </summary>
			Euclidean,

            /// <summary>
            /// Distance without diagonals, only horizontal and/or vertical path lines.
            /// </summary>
			Manhattan
        }

        /// <summary>
        /// Find a path between two points.
        /// </summary>
        /// <param name="grid">Grid to search.</param>
        /// <param name="startPos">Starting position.</param>
		/// <param name="targetPos">Ending position.</param>
        /// <param name="distance">The type of distance, Euclidean or Manhattan.</param>
        /// <param name="ignorePrices">If true, will ignore tile price (how much it "cost" to walk on).</param>
        /// <returns>List of points that represent the path to walk.</returns>
		public static List<Tile> FindPath(Grid grid, Tile startTile, Tile targetTile, DistanceType distance = DistanceType.Euclidean, bool ignorePrices = true)
        {
            List<Tile> openSet = new List<Tile>();
            HashSet<Tile> closedSet = new HashSet<Tile>();
            openSet.Add(startTile);

            while (openSet.Count > 0)
            {
                Tile currentTile = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentTile.fCost || openSet[i].fCost == currentTile.fCost && openSet[i].hCost < currentTile.hCost)
                    {
                        currentTile = openSet[i];
                    }
                }

                openSet.Remove(currentTile);
                closedSet.Add(currentTile);

                if (currentTile == targetTile)
                {
                    return RetracePath(grid, startTile, targetTile);
                }

                foreach (Tile neighbour in grid.GetNeighbours(currentTile, distance))
                {
                    if (!neighbour.Walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentTile.gCost + GetDistance(currentTile, neighbour) * (ignorePrices ? 1 : (int)(10.0f * neighbour.price));
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetTile);
                        neighbour.parent = currentTile;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Retrace path between two points.
        /// </summary>
        /// <param name="grid">Grid to work on.</param>
        /// <param name="startTile">Starting node.</param>
        /// <param name="endTile">Ending (target) node.</param>
        /// <returns>Retraced path between nodes.</returns>
        private static List<Tile> RetracePath(Grid grid, Tile startTile, Tile endTile)
        {
            List<Tile> path = new List<Tile>();
            Tile currentTile = endTile;

            while (currentTile != startTile)
            {
                path.Add(currentTile);
                currentTile = currentTile.parent;
            }
            path.Reverse();
            return path;
        }

        /// <summary>
        /// Get distance between two nodes.
        /// </summary>
        /// <param name="nodeA">First node.</param>
        /// <param name="nodeB">Second node.</param>
        /// <returns>Distance between nodes.</returns>
        private static int GetDistance(Tile nodeA, Tile nodeB)
        {
            int dstX = System.Math.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = System.Math.Abs(nodeA.gridY - nodeB.gridY);
            return (dstX > dstY) ?
                14 * dstY + 10 * (dstX - dstY) :
                14 * dstX + 10 * (dstY - dstX);
        }
    }

}
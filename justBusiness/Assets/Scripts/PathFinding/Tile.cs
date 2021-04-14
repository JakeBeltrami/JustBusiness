/**
 * Represent a single node in the pathfinding grid.
 * Based on code and tutorial by Sebastian Lague (https://www.youtube.com/channel/UCmtyQOKKmrMVaKuRXz02jbQ).
 *   
 * Author: Ronen Ness.
 * Since: 2016. 
*/

using UnityEngine;

namespace PathFind
{

    /// <summary>
    /// Represent a single node in the pathfinding grid.
    /// </summary>
    public class Tile
    {
        // is this node walkable?
        private bool walkable;
        private bool occupied;
        public int gridX;
        public int gridY;
        public float price;
        public Vector2 position;

        // calculated values while finding path
        public int gCost;
        public int hCost;
        public Tile parent;

        public Vector2 Center { get { return position; } }
        public bool Walkable { get { return walkable; } set { walkable = value; } }
        public bool Occupied { get { return occupied; } set { occupied = value; } }

        /// <summary>
        /// Create the grid node.
        /// </summary>
        /// <param name="_walkable">Is this tile walkable?</param>
        /// <param name="_gridX">Tile x index.</param>
        /// <param name="_gridY">Tile y index.</param>
        public Tile(bool _walkable, int _gridX, int _gridY, Vector2 _position)
        {
            walkable = _walkable;
            price = _walkable ? 1f : 0f;
            gridX = _gridX;
            gridY = _gridY;
            position = _position;
            occupied = false;
        }

        /// <summary>
        /// Updates the grid node.
        /// </summary>
        /// <param name="_walkable">Is this tile walkable?</param>
        /// <param name="_gridX">Tile x index.</param>
        /// <param name="_gridY">Tile y index.</param>
        public void Update(bool _walkable, int _gridX, int _gridY)
        {
            walkable = _walkable;
            price = _walkable ? 1f : 0f;
            gridX = _gridX;
            gridY = _gridY;
        }

        /// <summary>
        /// Get fCost of this node.
        /// </summary>
        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
    }
}
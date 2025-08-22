using UnityEngine;

namespace SGGames.Scripts.PathFindings
{
    /// <summary>
    /// Representation of node in grid. Node could be either enemy, player or any entities.
    /// </summary>
    public class Node
    {
        public Vector3Int GridPosition { get; }
        public Node Parent { get; }

        public Node(Vector3Int  gridPosition, Node parent = null)
        {
            GridPosition = gridPosition;
            Parent = parent;
        }
    }
}

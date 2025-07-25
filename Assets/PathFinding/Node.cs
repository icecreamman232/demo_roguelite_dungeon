using UnityEngine;

namespace SGGames.Script.PathFindings
{
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

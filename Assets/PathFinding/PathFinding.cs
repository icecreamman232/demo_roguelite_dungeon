using System;
using System.Collections.Generic;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.PathFindings
{
    public class PathFinding : MonoBehaviour
    {
        private Vector2Int[] Path;
        private Vector2Int m_previousGridPos;
        private Vector2Int m_selfPreviousGridPos;
        private int m_enemyInstanceId;
        private GridManager m_gridManager;

        private void Start()
        {
            m_enemyInstanceId = GetInstanceID();
            Path = Array.Empty<Vector2Int>();
            m_gridManager = ServiceLocator.GetService<GridManager>();
            // Vector2Int initialGridPos = m_gridManager.TilePosToGrid(
            //     m_gridManager.Tilemap.WorldToCell(transform.position));
            // m_gridManager.RegisterEnemy(m_enemyInstanceId, initialGridPos);
        }

        private void OnDestroy()
        {
            m_gridManager.UnregisterEnemy(m_enemyInstanceId);
        }
        
        public Vector2Int[] FindPath(Vector2Int start, Vector2Int end)
        {
            // Temporarily make our own position walkable during pathfinding
            m_gridManager.SetEnemyPositionWalkableForPathfinding(m_enemyInstanceId, true);

            Queue<Node> queue = new Queue<Node>();
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
            
            Vector3Int startPos = new Vector3Int(start.x, start.y, 0);
            
            Node startNode = new Node(startPos);
            queue.Enqueue(startNode);
            visited.Add(start);
            
            // Define possible movement directions (including diagonals)
            Vector2Int[] directions = new Vector2Int[]
            {
                new Vector2Int(0, 1),   // Up
                //new Vector2Int(1, 1),   // Up-Right (diagonal)
                new Vector2Int(1, 0),   // Right
                //new Vector2Int(1, -1),  // Down-Right (diagonal)
                new Vector2Int(0, -1),  // Down
                //new Vector2Int(-1, -1), // Down-Left (diagonal)
                new Vector2Int(-1, 0),  // Left
                //new Vector2Int(-1, 1)   // Up-Left (diagonal)
            };

            Vector2Int[] foundPath = null;

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();
                Vector2Int currentPos = new Vector2Int(current.GridPosition.x, current.GridPosition.y);

                if (currentPos.x == end.x && currentPos.y == end.y)
                {
                    foundPath = ReconstructPath(current);
                    break;
                }

                foreach (Vector2Int dir in directions)
                {
                    Vector2Int nextPos = new Vector2Int(currentPos.x + dir.x, currentPos.y + dir.y);
                    
                    // Check if the next position is valid using shared grid
                    if (nextPos.x >= 0 && nextPos.x < m_gridManager.RoomWidth &&
                        nextPos.y >= 0 && nextPos.y < m_gridManager.RoomHeight &&
                        m_gridManager.GetWalkable(nextPos.x, nextPos.y) &&
                        !visited.Contains(nextPos))
                    {
                        // // For diagonal movement, also check that both adjacent cells are walkable
                        // if (Mathf.Abs(dir.x) == 1 && Mathf.Abs(dir.y) == 1)
                        // {
                        //     bool horizontalNeighborWalkable = m_gridManager.GetWalkable(currentPos.x + dir.x, currentPos.y);
                        //     bool verticalNeighborWalkable = m_gridManager.GetWalkable(currentPos.x, currentPos.y + dir.y);
                        //
                        //     if (!horizontalNeighborWalkable || !verticalNeighborWalkable)
                        //     {
                        //         continue;
                        //     }
                        // }
                    
                        visited.Add(nextPos);
                        Vector3Int nextPosV3 = new Vector3Int(nextPos.x, nextPos.y, 0);
                        Node nextNode = new Node(nextPosV3, current);
                        queue.Enqueue(nextNode);
                    }
                }
            }

            // Restore our position as non-walkable
            m_gridManager.SetEnemyPositionWalkableForPathfinding(m_enemyInstanceId, false);

            return foundPath;
        }
        
        private Vector2Int[] ReconstructPath(Node endNode)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            Node current = endNode;
            
            while (current != null)
            {
                path.Add(new Vector2Int(current.GridPosition.x, current.GridPosition.y));
                current = current.Parent;
            }
        
            path.Reverse();
            return path.ToArray();
        }

        // private void Update()
        // {
        //     if(m_gridManager == null) return;
        //     UpdateSelfToGrid();
        // }
        
        private void UpdateSelfToGrid()
        {
            var targetPos = transform.position;
            var tilePos = m_gridManager.Tilemap.WorldToCell(targetPos);
            var tile = m_gridManager.Tilemap.GetTile(tilePos);

            if (tile == null)
            {
                var gridPos = m_gridManager.TilePosToGrid(tilePos);
                if (gridPos != m_selfPreviousGridPos)
                {
                    m_gridManager.UpdateEnemyPosition(m_enemyInstanceId, gridPos);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (m_gridManager == null) return;
            
            if (Path != null && Path.Length > 0)
            {
                for (int i = 0; i < Path.Length - 1; i++)
                {
                    var gridPos = Path[i];
                    var tilePos = m_gridManager.GridPosToTile(gridPos);
                    var worldPos = m_gridManager.Tilemap.CellToWorld(new Vector3Int(tilePos.x, tilePos.y, 0));
                    worldPos.x += GridManager.k_TileMapOffset;
                    worldPos.y += GridManager.k_TileMapOffset;
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(worldPos, Vector3.one * 0.5f);
                }
            }
        }
    }
}
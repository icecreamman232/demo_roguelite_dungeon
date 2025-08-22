using System.Collections.Generic;
using SGGames.Scripts.Core;
using SGGames.Scripts.Events;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace SGGames.Scripts.PathFindings
{
    public class GridManager : MonoBehaviour, IGameService
    {
        [Header("Grid Settings")]
        [SerializeField] private int m_roomWidth;
        [SerializeField] private int m_roomHeight;
        [SerializeField] private Tilemap m_tilemap;
        [SerializeField] private ObjectPooler m_effectTilePooler;
        [SerializeField] private DisplayEffectTileEvent m_displayEffectTileEvent;
        [Header("Debug")]
        [SerializeField] private bool m_showGrid;
        
        public const float k_TileMapOffset = 0.5f;
        private GridController m_gridController;
        private Dictionary<int, Vector2Int> m_enemyPositions;
        private Dictionary<int, Vector2Int> m_previousEnemyPositions;
        private List<GameObject> m_effectTiles = new List<GameObject>();

        public Tilemap Tilemap
        {
            get => m_tilemap;
            set
            {
                m_tilemap = value;
                if (m_tilemap != null)
                {
                    ReadTileMapIntoGrid();
                }
            }
        }

        public int RoomWidth => m_roomWidth;
        public int RoomHeight => m_roomHeight;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<GridManager>(this);
            m_displayEffectTileEvent.AddListener(OnDisplayEffectTile);
            m_enemyPositions = new Dictionary<int, Vector2Int>();
            m_previousEnemyPositions = new Dictionary<int, Vector2Int>();
            Initialize();
        }

        private void OnDestroy()
        {
            m_displayEffectTileEvent.RemoveListener(OnDisplayEffectTile);
        }

        private void Initialize()
        {
            m_gridController = new GridController(m_roomWidth, m_roomHeight);
        }
        
        private void ReadTileMapIntoGrid()
        {
            for (int y = -m_roomHeight/2; y < m_roomHeight/2; y++)
            {
                for (int x = -m_roomWidth/2; x < m_roomWidth/2; x++)
                {
                    var pos = new Vector3(x + k_TileMapOffset, y + k_TileMapOffset, 0);
                    var tilePos = m_tilemap.WorldToCell(pos);
                    
                    var tile = m_tilemap.GetTile(tilePos);
                    
                    var gridPos = TilePosToGrid(tilePos);
                    m_gridController.SetWalkable(gridPos.x ,gridPos.y,tile == null);
                }
            }
        }

        public Vector3Int WorldPosToTile(Vector3 worldPos)
        {
            return m_tilemap.WorldToCell(worldPos);
        }

        public Vector3 GetSnapPosition(Vector3 position)
        {
            var tilePos = m_tilemap.WorldToCell(position);
            var worldPos = m_tilemap.CellToWorld(new Vector3Int(tilePos.x, tilePos.y, 0));
            worldPos.x += k_TileMapOffset;
            worldPos.y += k_TileMapOffset;
            return worldPos;
        }
        
        public Vector3 GridPosToWorld(Vector2Int gridPos)
        {
            var tilePos = GridPosToTile(gridPos);
            return m_tilemap.CellToWorld(new Vector3Int(tilePos.x, tilePos.y, 0));
        }
        
        public Vector2Int TilePosToGrid(Vector3Int tilePos)
        {
            return new Vector2Int (tilePos.x + m_roomWidth/2, tilePos.y + m_roomHeight/2);
        }
        
        public Vector2Int GridPosToTile(Vector2Int gridPos)
        {
            return new Vector2Int(gridPos.x - m_roomWidth/2, gridPos.y - m_roomHeight/2);
        }
        
        /// <summary>
        /// Finds the walkable status of a grid position.
        /// </summary>
        /// <param name="x">Grid Position X</param>
        /// <param name="y">Grid Position Y</param>
        /// <returns></returns>
        public bool GetWalkable(int x, int y)
        {
            if (x >= 0 && x < m_roomWidth && y >= 0 && y < m_roomHeight)
            {
                return m_gridController.Grid[x, y];
            }
            return false;
        }

        public void RegisterEnemy(int instanceID, Vector2Int gridPos)
        {
            if (m_enemyPositions.ContainsKey(instanceID)) return;
            m_enemyPositions[instanceID] = gridPos;
            m_previousEnemyPositions[instanceID] = gridPos;
                
            //Mark the position as occupied
            m_gridController.SetWalkable(gridPos.x ,gridPos.y,false);
        }

        public void UnregisterEnemy(int instanceID)
        {
            if (!m_enemyPositions.ContainsKey(instanceID)) return;

            var lastPos = m_enemyPositions[instanceID];
            m_gridController.SetWalkable(lastPos.x, lastPos.y, true);

            m_enemyPositions.Remove(instanceID);
            m_previousEnemyPositions.Remove(instanceID);
        }
        
        // Method to temporarily mark a position as walkable for a specific enemy during pathfinding
        public void SetEnemyPositionWalkableForPathfinding(int enemyInstanceId, bool walkable)
        {
            if (m_enemyPositions.ContainsKey(enemyInstanceId))
            {
                Vector2Int enemyPos = m_enemyPositions[enemyInstanceId];
                m_gridController.SetWalkable(enemyPos.x, enemyPos.y, walkable);
            }
        }


        public void UpdateEnemyPosition(int instanceID, Vector2Int gridPos)
        {
            var prevPosition = m_enemyPositions[instanceID];
            if (prevPosition != gridPos)
            {
                m_gridController.SetWalkable(prevPosition.x, prevPosition.y, true);
            }

            m_previousEnemyPositions[instanceID] = prevPosition;
            m_enemyPositions[instanceID] = gridPos;
        }
        
        private void OnDisplayEffectTile(EffectTileEventData effectTileEventData)
        {
            if (m_effectTiles.Count > 0)
            {
                foreach (var tile in m_effectTiles)
                {
                    tile.SetActive(false);
                }
            }

            for (int i = 0; i < effectTileEventData.Position.Count; i++)
            {
                var effectTileGO = m_effectTilePooler.GetPooledGameObject();
                effectTileGO.transform.position = effectTileEventData.Position[i];
                effectTileGO.SetActive(true);
                m_effectTiles.Add(effectTileGO);
            }
        }
        
        
        private void OnDrawGizmos()
        {
            if (!m_showGrid) return;
            
            if (m_gridController == null) return;
        
            for (int y = -m_roomHeight/2; y < m_roomHeight/2; y++)
            {
                for (int x = -m_roomWidth/2; x < m_roomWidth/2; x++)
                {
                    var posX = x;
                    var posY = y;
                    var pos = new Vector3(posX + k_TileMapOffset, posY + k_TileMapOffset, 0);
                    Vector2Int gridPos = new Vector2Int(x + m_roomWidth/2, y + m_roomHeight/2);
                
                    // Check if position is occupied by an enemy
                    bool occupiedByEnemy = false;
                    foreach (var enemyPos in m_enemyPositions.Values)
                    {
                        if (enemyPos == gridPos)
                        {
                            occupiedByEnemy = true;
                            break;
                        }
                    }
                
                    if (occupiedByEnemy)
                    {
                        Gizmos.color = Color.blue; // Enemy positions
                    }
                    else
                    {
                        Gizmos.color = m_gridController.Grid[gridPos.x, gridPos.y] ? Color.yellow : Color.red;
                    }
                
                    Gizmos.DrawCube(pos, Vector3.one * 0.5f);
                }
            }
        }
    }
}

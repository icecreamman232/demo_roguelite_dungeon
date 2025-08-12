
using System;
using System.Collections;
using System.Collections.Generic;
using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Managers;
using SGGames.Script.PathFindings;
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Script.Entity
{
    public class EnemyMovement : EntityMovement
    {
        [Header("Enemy Basic Settings")] 
        [SerializeField] private Global.MovementBehaviorType m_movementBehaviorType;
        [SerializeField] private EnemyData m_enemyData;
        [SerializeField] private LayerMask m_obstacleLayerMask;
        private const float k_RaycastDistance = 0.15f;

        [Header("Pathfinding Settings")]
        [SerializeField] private bool m_usePathfinding = false;
        [SerializeField] private PathFinding m_pathFinder;
        [SerializeField] private float m_pathUpdateInterval = 1f;
        [SerializeField] private float m_nextWaypointDistance = 0.1f;
        
        [Header("Advanced Movement Features")]
        [SerializeField] private bool m_useWallSliding = true;
        [SerializeField] private bool m_useWallProximityDetection = true;
        [SerializeField] private bool m_usePathOptimization = true;
        [SerializeField] private bool m_avoidOtherEnemies = true;
        
        [Header("Wall Sliding Settings")]
        [SerializeField] private float m_wallProximityThreshold = 1.0f;
        [SerializeField] private float m_cornerSlidingSpeed = 1.5f;
        
        [Header("Enemy Avoidance")]
        [SerializeField] private LayerMask m_enemyLayer;
        [SerializeField] private float m_enemyAvoidanceRadius = 1.0f;
        [SerializeField] private float m_enemyAvoidanceForce = 2.0f;
        
        [Header("Debug")]
        [SerializeField] private bool m_enableDebug = false;

        // Components and services
        private EnemyController m_controller;
        private BoxCollider2D m_collider2D;
        private RaycastHit2D m_collisionHit;
        private GridManager m_gridManager;

        // Target tracking
        private Transform m_target;

        // Pathfinding state
        private List<Vector3> m_waypoints = new List<Vector3>();
        private int m_currentWaypointIndex;
        private bool m_hasPath;
        private bool m_pathPending;
        private bool m_isDirectPathBlocked;
        private bool m_forcedPathfinding;
        private float m_lastPathRequestTime;
        
        //Stunning
        private bool m_isStunned;
        
        // Properties
        public Vector2 MoveDirection => m_movementDirection;
        public Global.MovementState CurrentMovementState => m_currentMovementState;
        
        #region Unity Lifecycle
        
        protected override void Awake()
        {
            var gameManager = ServiceLocator.GetService<GameManager>();
            gameManager.OnGamePauseCallback += OnGamePaused;
            gameManager.OnGameUnPauseCallback += OnGameResumed;

            m_collider2D = GetComponent<BoxCollider2D>();
            SetMovementType(Global.MovementType.Normal);
            base.Awake();
        }

        private void Start()
        {
            m_gridManager = ServiceLocator.GetService<GridManager>();
            
            // Get PathFinding component if not assigned and pathfinding is enabled
            if (m_usePathfinding && m_pathFinder == null)
            {
                m_pathFinder = GetComponent<PathFinding>();
                if (m_pathFinder == null)
                {
                    Debug.LogError($"PathFinding component not found on {gameObject.name}! Please attach a PathFinding component.");
                }
            }
        }
        
        private void OnDestroy()
        {
            var gameManager = ServiceLocator.GetService<GameManager>();
            if (gameManager != null)
            {
                gameManager.OnGamePauseCallback -= OnGamePaused;
                gameManager.OnGameUnPauseCallback -= OnGameResumed;
            }
        }

        #endregion

        #region Public API
        public void Initialize(EnemyController controller)
        {
            m_controller = controller;
        }
        
        public void ApplyStun(float duration)
        {
            //TODO: Here is simple stunning mechanic which there is only 1 stun instance could apply on the enemy
            //Can improve this with override mechanic which stun instance with longer duration will override the shorter one
            if (m_isStunned) return;
            StartCoroutine(OnBeingStunned(duration));
        }
        
        public void SetDirection(Vector2 dir)
        {
            m_movementDirection = dir;
        }

        public void StartMoving()
        {
            Debug.Log("Start Moving");
            SetMovementType(Global.MovementType.Normal);
            SetPermission(true);
            SetNextPosition();
            SetMovementState(Global.MovementState.Moving);
        }

        public void PauseMoving()
        {
            SetMovementType(Global.MovementType.Stop);
        }

        public void ResumeMoving()
        {
            SetMovementType(Global.MovementType.Normal);
        }

        public void StopMoving()
        {
            m_movementDirection = Vector2.zero;
            SetMovementType(Global.MovementType.Stop);
            SetMovementBehaviorType(Global.MovementBehaviorType.Normal);
            
            // Clear pathfinding state
            m_hasPath = false;
            m_waypoints.Clear();
            m_forcedPathfinding = false;
        }

        public void SetFollowingTarget(Transform followingTarget)
        {
            m_target = followingTarget;
            SetMovementType(Global.MovementType.Normal);
            SetMovementBehaviorType(Global.MovementBehaviorType.FollowingTarget);
        }

        public void SetMovementBehaviorType(Global.MovementBehaviorType movementBehaviorType)
        {
            m_movementBehaviorType = movementBehaviorType;
        }
        
       
        #endregion

        #region Movement Update Override

        protected override void OnFinishMovement()
        {
            //m_controller.FinishTurn();
            base.OnFinishMovement();
            SetPermission(false);
        }

        #endregion
        
        
        #region Pathfinding
        [ContextMenu("Find path")]
        public void TestPathFinding()
        {
            m_target = ServiceLocator.GetService<LevelManager>().Player.transform;
            SetNextPosition();
            SetMovementState(Global.MovementState.Moving);
        }

        private void SetNextPosition()
        {
            UpdatePath();
            m_nextPosition = m_waypoints[1];
        }
        
        private void UpdatePath()
        {
            if (!m_usePathfinding || m_pathFinder == null || m_gridManager == null) return;
            
            Vector2Int startPos = m_gridManager.TilePosToGrid(m_gridManager.Tilemap.WorldToCell(transform.position));
            Vector2Int endPos = m_gridManager.TilePosToGrid(m_gridManager.Tilemap.WorldToCell(m_target.position));
            
            Vector2Int[] gridPath = m_pathFinder.FindPath(startPos, endPos);
            
            if (gridPath != null && gridPath.Length > 0)
            {
                ProcessPath(gridPath);
                m_hasPath = true;
            }
            else
            {
                m_hasPath = false;
                m_waypoints.Clear();
            }
        }
        
        private void ProcessPath(Vector2Int[] gridPath)
        {
            m_waypoints.Clear();
            
            List<Vector3> rawWaypoints = new List<Vector3>();
            for (int i = 0; i < gridPath.Length; i++)
            {
                Vector2Int gridPos = gridPath[i];
                Vector2Int tilePos = m_gridManager.GridPosToTile(gridPos);
                Vector3 worldPos = m_gridManager.Tilemap.CellToWorld(new Vector3Int(tilePos.x, tilePos.y, 0));
                worldPos.x += GridManager.k_TileMapOffset;
                worldPos.y += GridManager.k_TileMapOffset;
                
                rawWaypoints.Add(worldPos);
            }
            
            m_waypoints = rawWaypoints;
        }
        
        private List<Vector3> PathSimplification(List<Vector3> path)
        {
            if (path.Count <= 2)
                return new List<Vector3>(path);
                
            List<Vector3> simplified = new List<Vector3>();
            simplified.Add(path[0]);
            
            int currentPoint = 0;
            
            while (currentPoint < path.Count - 1)
            {
                int furthestVisible = currentPoint + 1;
                
                for (int i = currentPoint + 2; i < path.Count; i++)
                {
                    if (HasLineOfSight(path[currentPoint], path[i]))
                    {
                        furthestVisible = i;
                    }
                    else
                    {
                        break;
                    }
                }
                
                simplified.Add(path[furthestVisible]);
                currentPoint = furthestVisible;
            }
            
            return simplified;
        }
        
        private bool HasLineOfSight(Vector3 start, Vector3 end)
        {
            Vector3 direction = end - start;
            float distance = direction.magnitude;
            direction.Normalize();
            
            int steps = Mathf.CeilToInt(distance / 0.5f);
            
            for (int i = 1; i < steps; i++)
            {
                Vector3 point = start + direction * (distance * i / steps);
                if (IsWallAt(point))
                {
                    return false;
                }
            }
            
            return true;
        }
        
        private bool IsWallAt(Vector3 position)
        {
            if (!m_usePathfinding || m_pathFinder == null || m_gridManager == null) return false;
            
            Vector3Int cellPos = m_gridManager.Tilemap.WorldToCell(position);
            Vector2Int gridPos = m_gridManager.TilePosToGrid(cellPos);
            
            if (gridPos.x >= 0 && gridPos.x < m_gridManager.RoomWidth &&
                gridPos.y >= 0 && gridPos.y < m_gridManager.RoomHeight)
            {
                return !m_gridManager.GetWalkable(gridPos.x, gridPos.y);
            }
            
            return true;
        }
        
        private int FindClosestWaypointIndex()
        {
            if (m_waypoints.Count == 0)
                return 0;
                
            int closestIndex = 0;
            float closestDistance = float.MaxValue;
            
            for (int i = 0; i < m_waypoints.Count; i++)
            {
                float distance = Vector3.Distance(transform.position, m_waypoints[i]);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }
            
            return Mathf.Min(closestIndex + 1, m_waypoints.Count - 1);
        }

        #endregion

        #region Game State Management

        protected override void OnGamePaused()
        {
            PauseMoving();
            base.OnGamePaused();
        }

        protected override void OnGameResumed()
        {
            ResumeMoving();
            base.OnGameResumed();
        }

        #endregion
        
        #region Stunning Mechanic

        private IEnumerator OnBeingStunned(float duration)
        {
            m_isStunned = true;
            PauseMoving();
            yield return new WaitForSeconds(duration);
            ResumeMoving();
            m_isStunned = false;
        }
        
        #endregion
        
        #region Debug

        private void OnDrawGizmos()
        {
            if (!m_enableDebug || !m_usePathfinding) return;
            
            // Draw waypoints and path
            if (m_waypoints.Count > 0 && m_hasPath)
            {
                for (int i = 0; i < m_waypoints.Count; i++)
                {
                    Gizmos.color = (i == m_currentWaypointIndex) ? Color.blue : Color.green;
                    Gizmos.DrawCube(m_waypoints[i], Vector3.one * 0.3f);
                    
                    if (i < m_waypoints.Count - 1)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawLine(m_waypoints[i], m_waypoints[i + 1]);
                    }
                }
            }
        }

        #endregion
    }
}
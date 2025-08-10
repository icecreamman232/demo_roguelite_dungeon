
using System;
using System.Collections;
using System.Collections.Generic;
using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Managers;
using SGGames.Script.PathFindings;
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
        private BoxCollider2D m_collider2D;
        private RaycastHit2D m_collisionHit;
        private GridManager m_gridManager;

        // Target tracking
        private Transform m_target;
        private const float k_MinPathRequestInterval = 0.2f;

        // Pathfinding state
        private List<Vector3> m_waypoints = new List<Vector3>();
        private int m_currentWaypointIndex;
        private bool m_hasPath;
        private bool m_pathPending;
        private bool m_isDirectPathBlocked;
        private bool m_forcedPathfinding;
        private float m_lastPathRequestTime;
        
        // Wall sliding state
        private bool m_isSliding;
        private Vector2 m_slidingDirection = Vector2.zero;
        private float m_slidingTimer;
        private const float k_MaxSlidingTime = 1.0f;

        //Stunning
        private bool m_isStunned;
        
        // Events
        public Action<bool> FlippingModelAction;
        
        // Properties
        public Vector2 MoveDirection => m_movementDirection;
        public bool IsUsingPathfinding => m_usePathfinding;
        
        #region Unity Lifecycle
        
        private void Awake()
        {
            var gameManager = ServiceLocator.GetService<GameManager>();
            gameManager.OnGamePauseCallback += OnGamePaused;
            gameManager.OnGameUnPauseCallback += OnGameResumed;

            m_collider2D = GetComponent<BoxCollider2D>();
            SetMovementType(Global.MovementType.Normal);
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
            SetMovementType(Global.MovementType.Normal);
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
            m_isSliding = false;
        }

        public void SetFollowingTarget(Transform followingTarget)
        {
            m_target = followingTarget;
            SetMovementType(Global.MovementType.Normal);
            SetMovementBehaviorType(Global.MovementBehaviorType.FollowingTarget);
            EnablePathfinding(true);
        }

        public void SetMovementBehaviorType(Global.MovementBehaviorType movementBehaviorType)
        {
            m_movementBehaviorType = movementBehaviorType;
        }

        /// <summary>
        /// Enable pathfinding mode - called by AI brain when complex navigation is needed
        /// </summary>
        public void EnablePathfinding(bool enable = true)
        {
            if (m_usePathfinding != enable)
            {
                m_usePathfinding = enable;
                
                if (enable)
                {
                    // Start pathfinding coroutine and update movement delegate
                    if (m_target != null)
                    {
                        StartCoroutine(UpdatePathRoutine());
                    }
                    // Force delegate refresh to use pathfinding version
                    SetMovementType(m_movementType);
                }
                else
                {
                    // Clear pathfinding state
                    StopAllCoroutines();
                    m_hasPath = false;
                    m_waypoints.Clear();
                    m_forcedPathfinding = false;
                    m_isSliding = false;
                    // Force delegate refresh to use normal version
                    SetMovementType(m_movementType);
                }
            }
        }

        /// <summary>
        /// Switch to pathfinding mode temporarily (automatically switches back when target is reached)
        /// </summary>
        public void UsePathfindingToTarget(Transform target)
        {
            m_target = target;
            m_forcedPathfinding = true;
            
            if (!m_usePathfinding)
            {
                EnablePathfinding(true);
            }
            else
            {
                RequestPathIfNeeded();
            }
        }
        
        #endregion

        #region Movement Update Override

        protected override void Update()
        {
            // Handle collision for normal movement only when not using pathfinding
            if (!m_usePathfinding && IsCollideWithObstacle())
            {
                StopMoving();
            }
            
            base.UpdateMovement();
        }

        protected override void UpdateMovement()
        {
            if (m_movementBehaviorType == Global.MovementBehaviorType.FollowingTarget)
            {
                if (m_usePathfinding)
                {
                    // Use pathfinding movement logic
                    UpdatePathfindingMovement();
                }
                else
                {
                    // Simple direct movement to target
                    transform.position = Vector3.MoveTowards(transform.position, m_target.position, Time.deltaTime);
                }
            }
            else
            {
                base.UpdateMovement();
            }
        }

        #endregion

        #region Pathfinding Movement Logic

        private void UpdatePathfindingMovement()
        {
            if (m_target == null) return;
            
            // Handle sliding only if enabled
            if (m_useWallSliding && m_isSliding)
            {
                HandleSliding();
                return;
            }
            
            // Check if there's a direct line of sight to the target
            Vector2 directionToTarget = ((Vector2)m_target.position - (Vector2)transform.position).normalized;
            
            // Apply local enemy avoidance if enabled
            if (m_avoidOtherEnemies)
            {
                Vector2 avoidanceForce = CalculateLocalEnemyAvoidance();
                if (avoidanceForce.magnitude > 0.1f)
                {
                    directionToTarget = (directionToTarget + avoidanceForce * m_enemyAvoidanceForce).normalized;
                }
            }
            
            // Test if there's a direct path to target
            m_isDirectPathBlocked = IsPathBlocked(transform.position, m_target.position);
            
            // Check if we're too close to walls (only if feature is enabled)
            bool tooCloseToWalls = m_useWallProximityDetection && IsNearWalls(transform.position, m_wallProximityThreshold);
            
            // If there's a clear path to the target and we're not in forced pathfinding mode
            if (!m_isDirectPathBlocked && !m_forcedPathfinding && !tooCloseToWalls)
            {
                m_hasPath = false;
                m_waypoints.Clear();
                
                if (!CheckCollisionForPathfinding(directionToTarget))
                {
                    SetDirection(directionToTarget);
                    // Use base movement to apply the direction
                    base.UpdateMovement();
                }
                else
                {
                    if (m_useWallSliding)
                    {
                        TrySlideAlongObstacle(directionToTarget);
                    }
                    
                    if (!m_isSliding)
                    {
                        m_forcedPathfinding = true;
                        RequestPathIfNeeded();
                    }
                }
                return;
            }
            
            // If we have a path and need to use it
            if (m_hasPath && m_waypoints.Count > 0)
            {
                Vector3 currentWaypoint = m_waypoints[m_currentWaypointIndex];
                if (Vector2.Distance(transform.position, currentWaypoint) < m_nextWaypointDistance)
                {
                    m_currentWaypointIndex++;
                    
                    if (m_currentWaypointIndex >= m_waypoints.Count)
                    {
                        m_hasPath = false;
                        m_forcedPathfinding = false;
                        RequestPathIfNeeded();
                        return;
                    }
                }

                Vector2 direction = ((Vector2)currentWaypoint - (Vector2)transform.position).normalized;
                
                // Apply local avoidance to waypoint direction as well
                if (m_avoidOtherEnemies)
                {
                    Vector2 avoidanceForce = CalculateLocalEnemyAvoidance();
                    if (avoidanceForce.magnitude > 0.1f)
                    {
                        direction = (direction + avoidanceForce * m_enemyAvoidanceForce * 0.5f).normalized;
                    }
                }
                
                if (!CheckCollisionForPathfinding(direction))
                {
                    SetDirection(direction);
                    // Use base movement to apply the direction
                    base.UpdateMovement();
                }
                else
                {
                    if (m_useWallSliding)
                    {
                        TrySlideAlongObstacle(direction);
                    }
                    
                    if (!m_isSliding)
                    {
                        RequestPathIfNeeded();
                    }
                }
            }
            else if ((m_isDirectPathBlocked || m_forcedPathfinding || tooCloseToWalls) && !m_pathPending)
            {
                RequestPathIfNeeded();
            }
        }

        #endregion

        #region Collision Detection

        protected virtual bool IsCollideWithObstacle()
        {
            m_collisionHit = Physics2D.BoxCast(transform.position, m_collider2D.size, 0, m_movementDirection, k_RaycastDistance, m_obstacleLayerMask);    
            return m_collisionHit.collider != null;
        }

        private bool CheckCollisionForPathfinding(Vector2 direction)
        {
            var hitResult = Physics2D.BoxCast(transform.position, Vector2.one * 0.8f, 0f, direction, 0.2f, m_obstacleLayerMask);
            if (hitResult.collider != null)
                return true;
                
            // Also check for enemy collisions for immediate collision detection
            if (m_avoidOtherEnemies)
            {
                var enemyHit = Physics2D.BoxCast(transform.position, Vector2.one * 0.8f, 0f, direction, 0.2f, m_enemyLayer);
                if (enemyHit.collider != null && enemyHit.collider.gameObject != gameObject)
                    return true;
            }
            
            return false;
        }

        private bool IsPathBlocked(Vector3 start, Vector3 end)
        {
            Vector2 direction = ((Vector2)end - (Vector2)start).normalized;
            float distance = Vector2.Distance(start, end);
            
            RaycastHit2D hitCenter = Physics2D.Raycast((Vector2)start, direction, distance, m_obstacleLayerMask);
            if (hitCenter.collider != null)
                return true;
                
            Vector2 perpendicular = new Vector2(-direction.y, direction.x).normalized;
            float checkWidth = 0.4f;
            
            RaycastHit2D hitLeft = Physics2D.Raycast((Vector2)start + perpendicular * checkWidth, direction, distance, m_obstacleLayerMask);
            if (hitLeft.collider != null)
                return true;
                
            RaycastHit2D hitRight = Physics2D.Raycast((Vector2)start - perpendicular * checkWidth, direction, distance, m_obstacleLayerMask);
            if (hitRight.collider != null)
                return true;
            
            return false;
        }

        private bool IsNearWalls(Vector3 position, float threshold)
        {
            if (!m_useWallProximityDetection) return false;
            
            Vector2[] directions = new Vector2[]
            {
                Vector2.up, new Vector2(0.7f, 0.7f).normalized, Vector2.right,
                new Vector2(0.7f, -0.7f).normalized, Vector2.down, new Vector2(-0.7f, -0.7f).normalized,
                Vector2.left, new Vector2(-0.7f, 0.7f).normalized
            };
            
            for (int i = 0; i < directions.Length; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast((Vector2)position, directions[i], threshold, m_obstacleLayerMask);
                if (hit.collider != null)
                {
                    return true;
                }
            }
            
            return false;
        }

        #endregion

        #region Enemy Avoidance

        private Vector2 CalculateLocalEnemyAvoidance()
        {
            Vector2 avoidanceForce = Vector2.zero;
            
            // Find nearby enemies using physics overlap (this is for immediate steering, not pathfinding)
            Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, m_enemyAvoidanceRadius, m_enemyLayer);
            
            foreach (Collider2D enemyCollider in nearbyEnemies)
            {
                if (enemyCollider.gameObject != gameObject)
                {
                    Vector2 directionAway = ((Vector2)transform.position - (Vector2)enemyCollider.transform.position);
                    float distance = directionAway.magnitude;
                    
                    if (distance > 0.1f && distance < m_enemyAvoidanceRadius)
                    {
                        float avoidanceStrength = (m_enemyAvoidanceRadius - distance) / m_enemyAvoidanceRadius;
                        avoidanceForce += directionAway.normalized * avoidanceStrength;
                    }
                }
            }
            
            return avoidanceForce;
        }

        #endregion

        #region Wall Sliding

        private void TrySlideAlongObstacle(Vector2 blockedDirection)
        {
            if (!m_useWallSliding) return;
            
            Vector2 rightPerpendicular = new Vector2(-blockedDirection.y, blockedDirection.x);
            Vector2 leftPerpendicular = new Vector2(blockedDirection.y, -blockedDirection.x);
            
            bool canSlideRight = !CheckCollisionForPathfinding(rightPerpendicular);
            bool canSlideLeft = !CheckCollisionForPathfinding(leftPerpendicular);
            
            if (canSlideRight || canSlideLeft)
            {
                m_isSliding = true;
                m_slidingTimer = 0f;
                
                if (canSlideRight && canSlideLeft)
                {
                    Vector2 toTarget = (Vector2)m_target.position - (Vector2)transform.position;
                    float dotRight = Vector2.Dot(rightPerpendicular, toTarget.normalized);
                    float dotLeft = Vector2.Dot(leftPerpendicular, toTarget.normalized);
                    
                    m_slidingDirection = (dotRight > dotLeft) ? rightPerpendicular : leftPerpendicular;
                }
                else
                {
                    m_slidingDirection = canSlideRight ? rightPerpendicular : leftPerpendicular;
                }
            }
        }
        
        private void HandleSliding()
        {
            if (!m_useWallSliding)
            {
                m_isSliding = false;
                return;
            }
            
            m_slidingTimer += Time.deltaTime;
            
            Vector2 directionToTarget = ((Vector2)m_target.position - (Vector2)transform.position).normalized;
            bool canMoveToTarget = !CheckCollisionForPathfinding(directionToTarget) && !IsPathBlocked(transform.position, m_target.position);
            
            if (m_slidingTimer >= k_MaxSlidingTime || canMoveToTarget)
            {
                m_isSliding = false;
                
                if (!canMoveToTarget && m_usePathfinding)
                {
                    m_forcedPathfinding = true;
                    RequestPathIfNeeded();
                }
                return;
            }
            
            if (!CheckCollisionForPathfinding(m_slidingDirection))
            {
                SetDirection(m_slidingDirection);
                // Apply sliding speed multiplier by directly moving transform
                transform.Translate(m_movementDirection * (m_cornerSlidingSpeed * Time.deltaTime));
                
                Vector2 diagonalDirection = (m_slidingDirection + directionToTarget).normalized;
                if (!CheckCollisionForPathfinding(diagonalDirection))
                {
                    SetDirection(diagonalDirection);
                }
            }
            else
            {
                m_slidingDirection = -m_slidingDirection;
                
                if (CheckCollisionForPathfinding(m_slidingDirection))
                {
                    m_isSliding = false;
                    if (m_usePathfinding)
                    {
                        m_forcedPathfinding = true;
                        RequestPathIfNeeded();
                    }
                }
            }
        }

        #endregion

        #region Pathfinding

        private void RequestPathIfNeeded()
        {
            if (!m_usePathfinding) return;
            
            if (Time.time - m_lastPathRequestTime >= k_MinPathRequestInterval && !m_pathPending)
            {
                StartCoroutine(RequestPath());
                m_lastPathRequestTime = Time.time;
            }
        }

        private IEnumerator UpdatePathRoutine()
        {
            while (m_usePathfinding)
            {
                yield return new WaitForSeconds(m_pathUpdateInterval);
                
                if (m_target != null && m_pathFinder != null && !m_pathPending && !m_isSliding &&
                    (m_isDirectPathBlocked || m_forcedPathfinding || (m_useWallProximityDetection && IsNearWalls(transform.position, m_wallProximityThreshold))))
                {
                    RequestPathIfNeeded();
                }
            }
        }

        private IEnumerator RequestPath()
        {
            m_pathPending = true;
            yield return null;
            UpdatePath();
            m_pathPending = false;
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
                m_currentWaypointIndex = FindClosestWaypointIndex();
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
            
            m_waypoints = m_usePathOptimization ? PathSimplification(rawWaypoints) : rawWaypoints;
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
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawLine(m_waypoints[i], m_waypoints[i + 1]);
                    }
                }
            }
            
            // Draw line to target with status colors
            if (m_target != null)
            {
                bool nearWalls = m_useWallProximityDetection && IsNearWalls(transform.position, m_wallProximityThreshold);
                Gizmos.color = (m_useWallSliding && m_isSliding) ? Color.magenta : 
                              (m_isDirectPathBlocked ? Color.red : 
                              (m_forcedPathfinding ? Color.yellow : 
                              (nearWalls ? Color.cyan : Color.green)));
                               
                Gizmos.DrawLine(transform.position, m_target.position);
                
                // Draw enemy avoidance radius
                if (m_avoidOtherEnemies)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere(transform.position, m_enemyAvoidanceRadius);
                }
            }
        }

        #endregion
    }
}
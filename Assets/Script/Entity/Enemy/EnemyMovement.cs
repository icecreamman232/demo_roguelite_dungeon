using System.Collections;
using System.Collections.Generic;
using SGGames.Scripts.Core;
using SGGames.Scripts.Managers;
using SGGames.Scripts.PathFindings;
using UnityEngine;

namespace SGGames.Scripts.Entities
{
    public class EnemyMovement : EntityMovement
    {
        [Header("Enemy Basic Settings")] 
        [SerializeField] private LayerMask m_obstacleLayerMask;
        
        [Header("Pathfinding Settings")]
        [SerializeField] private bool m_usePathfinding = false;
        [SerializeField] private PathFinding m_pathFinder;
        [SerializeField] private float m_nextWaypointDistance = 0.1f;
        
        [Header("Debug")]
        [SerializeField] private bool m_enableDebug = false;
        
        private EnemyController m_controller;
        private RaycastHit2D m_collisionHit;
        private GridManager m_gridManager;
        private Transform m_target;
        private bool m_isStunned;
        private int m_numberStepPerTurn;
        private int m_currentStep;

        // Pathfinding state
        private List<Vector3> m_waypoints = new List<Vector3>();
        private int m_currentWaypointIndex;
        private bool m_hasPath;
        
        
        
        // Properties
        public Global.MovementState CurrentMovementState => m_currentMovementState;
        
        #region Unity Lifecycle
        
        protected override void Awake()
        {
            InternalInitialize();
            base.Awake();
        }

        private void Start()
        {
            ExternalInitialize();
        }

        #endregion
        
        #region Initialize
        
        public void Initialize(EnemyController controller)
        {
            m_controller = controller;
        }
        
        private void InternalInitialize()
        {
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

        private void ExternalInitialize()
        {
            m_gridManager = ServiceLocator.GetService<GridManager>();
            var gameManager = ServiceLocator.GetService<GameManager>();
            gameManager.OnGamePauseCallback += OnGamePaused;
            gameManager.OnGameUnPauseCallback += OnGameResumed;
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
            UpdatePath();
            m_numberStepPerTurn = m_controller.Data.StepPerTurn < m_waypoints.Count
                ? m_controller.Data.StepPerTurn
                : m_waypoints.Count - 1;
            m_currentStep = 1;
            m_nextPosition = m_waypoints[m_currentStep];
            
            //Pre-check if the next position is empty or not
            if (HasPlayerAtThisPosition(m_nextPosition) || HasEnemyAtThisPosition(m_nextPosition))
            {
                Debug.Log("Next position is not empty!");
                m_nextPosition = m_currentPosition;
                SetMovementState(Global.MovementState.DelayAfterMoving);
                return;
            }
            
            SetMovementState(Global.MovementState.Moving);
        }

        public void PauseMoving()
        {
            
        }

        public void ResumeMoving()
        {
           
        }

        public void StopMoving()
        {
            m_movementDirection = Vector2.zero;
            // Clear pathfinding state
            m_hasPath = false;
            m_waypoints.Clear();
        }

        public void SetFollowingTarget(Transform followingTarget)
        {
            m_target = followingTarget;
            StartMoving();
        }

        #endregion

        protected override void UpdateMovement()
        {
            m_lerpValue += Time.deltaTime * k_MovementSpeed;
            transform.position = Vector3.Lerp(m_currentPosition, m_nextPosition, m_lerpValue);
            if (m_lerpValue >= 1)
            {
                m_lerpValue = 0;
                m_currentStep++;
                //Snap to the latest position
                m_currentPosition = m_nextPosition;
                
                //Movement rules:
                //1. If the final position is not empty, then stop moving
                //2. If the final position is empty, then move to the next position
                if (m_currentStep > m_numberStepPerTurn || HasEnemyAtThisPosition(m_waypoints[m_currentStep]) || HasPlayerAtThisPosition(m_waypoints[m_currentStep]))
                {
                    SetMovementState(Global.MovementState.DelayAfterMoving);
                }
                else
                {
                    m_nextPosition = m_waypoints[m_currentStep];
                }
            }
        }

        private bool HasEnemyAtThisPosition(Vector3 position)
        {
            var result = Physics2D.OverlapBox(position, Vector3.one * 0.7f, 0, LayerManager.EnemyMask); 
            return result != null;
        }

        private bool HasPlayerAtThisPosition(Vector3 position)
        {
            var result = Physics2D.OverlapBox(position, Vector3.one * 0.7f, 0, LayerManager.PlayerMask); 
            return result != null;
        }

        #region Pathfinding
        
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
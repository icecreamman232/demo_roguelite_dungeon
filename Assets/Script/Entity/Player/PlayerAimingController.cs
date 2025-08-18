using System;
using SGGames.Script.Core;
using SGGames.Script.Managers;
using UnityEngine;

namespace SGGames.Script.Entity
{
    [Serializable]
    public class AimingData
    {
        public Vector3 AimDirection;
        public float AimAngle;
    }
    
    public class PlayerAimingController : MonoBehaviour
    {
        [SerializeField] private AimingData m_aimingData;
        
        public AimingData AimingData => m_aimingData;
        
        public Action<AimingData> OnAimingDataChanged;
        
        private void Start()
        {
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.WorldMousePositionUpdate += OnWorldMousePositionChanged;
        }

        private void OnWorldMousePositionChanged(Vector3 worldMousePosition)
        {
            m_aimingData.AimDirection = (worldMousePosition - transform.position).normalized;
            m_aimingData.AimAngle = Mathf.Atan2(m_aimingData.AimDirection.y, m_aimingData.AimDirection.x) * Mathf.Rad2Deg - 90f;
            
            // Snap to 4 directions (90-degree increments)
            float snappedAngle = Mathf.Round(m_aimingData.AimAngle / 90f) * 90f;
            
            // Update aim direction to match the snapped angle
            float radians = (snappedAngle + 90f) * Mathf.Deg2Rad;
            
            m_aimingData.AimDirection = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0).normalized;
            
            var x = Mathf.Clamp(m_aimingData.AimDirection.x,-1, 1);
            var y = Mathf.Clamp(m_aimingData.AimDirection.y,-1, 1);
            m_aimingData.AimDirection = new Vector3 (x, y, 0).normalized;
            
            m_aimingData.AimAngle = snappedAngle;
            
            OnAimingDataChanged?.Invoke(m_aimingData);
        }
    }
}

using System;
using UnityEngine;

namespace SGGames.Scripts.Weapons
{
    [Serializable]
    public class FixedDirectionTargetingStrategy : ITargetingStrategy
    {
        [SerializeField] private Vector3 m_fixedDirection = Vector3.right;
    
        public Vector3 GetAimDirection(Transform weaponTransform, Transform targetTransform)
        {
            return m_fixedDirection.normalized;
        }
    }
}

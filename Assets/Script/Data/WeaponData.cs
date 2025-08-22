using System;
using UnityEngine;

namespace SGGames.Scripts.Data
{
    [CreateAssetMenu(menuName = "SGGames/Data/Weapon",fileName = "New Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private float m_coolDown;
        [SerializeField] private int m_projectilePerShot = 1;
        [SerializeField] private ShotProperty[] m_shotProperties;
        
        public float Cooldown => m_coolDown;
        public int ProjectilePerShot => m_projectilePerShot;
        
        public ShotProperty[] ShotProperties => m_shotProperties;
    }

    /// <summary>
    /// Define the angle, speed for each shot (projectile)
    /// </summary>
    [Serializable]
    public class ShotProperty
    {
        public Vector3 OffsetPosition;

        /// <summary>
        /// Bonus speed for a projectile that will be added in original speed
        /// </summary>
        public float BonusSpeed;
    }
}

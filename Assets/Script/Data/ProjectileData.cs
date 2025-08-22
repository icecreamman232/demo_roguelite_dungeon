using UnityEngine;

namespace SGGames.Scripts.Data
{
    [CreateAssetMenu(fileName = "Projectile Data", menuName = "SGGames/Data/Projectile")]
    public class ProjectileData : ScriptableObject
    {
        [SerializeField] private float m_speed;
        [SerializeField] private float m_range;
        
        public float Speed => m_speed;
        public float Range => m_range;
    }
}

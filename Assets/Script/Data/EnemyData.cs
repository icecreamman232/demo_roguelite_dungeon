using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(menuName = "SGGames/Enemy Data",fileName = "Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private float m_maxHealth;
        [SerializeField] private float m_moveSpeed;
        
        public float MaxHealth => m_maxHealth;
        public float MoveSpeed => m_moveSpeed;
    }
}


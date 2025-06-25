using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "Player Data", menuName = "SGGame/Player Data")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private float m_maxHealth;
        [SerializeField] private float m_moveSpeed;
        
        public float MaxHealth => m_maxHealth;
        public float MoveSpeed => m_moveSpeed;
    }
}


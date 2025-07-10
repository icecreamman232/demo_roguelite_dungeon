using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "Player Data", menuName = "SGGames/Player Data")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private float m_maxHealth;
        [SerializeField] private float m_moveSpeed;
        [Header("Dash Data")]
        [SerializeField] private float m_dashDistance;
        [SerializeField] private float m_dashSpeed;
        [SerializeField] private float m_dashCooldown;
        [SerializeField] private int m_staminaCostForDash;
        
        public float MaxHealth => m_maxHealth;
        public float MoveSpeed => m_moveSpeed;
        public float DashDistance => m_dashDistance;
        public float DashSpeed => m_dashSpeed;
        public float DashCooldown => m_dashCooldown;
        
        public int StaminaCostForDash => m_staminaCostForDash;
    }
}


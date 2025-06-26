using SGGames.Script.Data;
using UnityEngine;

namespace SGGames.Script.Entity
{
    public class EnemyMovement : EntityMovement
    {
        [Header("Enemy")]
        [SerializeField] private EnemyData m_enemyData;

        private void Awake()
        {
            m_moveSpeed = m_enemyData.MoveSpeed;
        }
    }
}

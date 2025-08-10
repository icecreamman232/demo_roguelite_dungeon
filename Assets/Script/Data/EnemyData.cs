using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(menuName = "SGGames/Enemy Data",fileName = "Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private Global.EnemyGrade m_enemyGrade;
        [SerializeField] private Global.EnemyProperties m_enemyProperties;
        [SerializeField] private float m_maxHealth;

        public Global.EnemyGrade EnemyGrade => m_enemyGrade;

        public Global.EnemyProperties EnemyProperties => m_enemyProperties;
        public float MaxHealth => m_maxHealth;
    }
}


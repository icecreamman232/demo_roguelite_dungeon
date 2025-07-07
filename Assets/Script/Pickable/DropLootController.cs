using SGGames.Script.Data;
using SGGames.Script.HealthSystem;
using UnityEngine;

namespace SGGames.Script.Pickable
{
    public class DropLootController : MonoBehaviour
    {
        [SerializeField] private LootTable m_lootTable;
        private Health m_health;

        private void Awake()
        {
            m_health = GetComponentInParent<Health>();
            m_health.OnDeath += SpawnLoot;
        }

        private void SpawnLoot()
        {
            m_health.OnDeath -= SpawnLoot;
            
            
            
        }
    }
}


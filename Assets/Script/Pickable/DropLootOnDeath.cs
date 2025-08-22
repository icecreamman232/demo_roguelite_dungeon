using SGGames.Scripts.Data;
using SGGames.Scripts.Events;
using SGGames.Scripts.HealthSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SGGames.Script.Pickable
{
    public class DropLootOnDeath : MonoBehaviour
    {
        [SerializeField] private CurrencyDropsEvent m_currencyDropsEvent;
        [SerializeField] private LootTable m_lootTable;
        private Health m_health;
        
        private void Awake()
        {
            m_health = GetComponentInParent<Health>();
            if (m_health)
            {
                m_health.OnDeath += SpawnLoot;
            }
        }
        
        [ContextMenu("Spawn Loot")]
        public void SpawnLoot()
        {
            if (m_health)
            {
                if (m_health.CanRevive) return;
                m_health.OnDeath -= SpawnLoot;
            }
            
            foreach (var loot in m_lootTable.GetLootTable())
            {
                var amount = Random.Range(loot.MinAmount, loot.MaxAmount);
                var hostPosition = m_health ? m_health.transform.position : transform.position;
                m_currencyDropsEvent.Raise(new CurrencyDropData
                {
                    ItemID = loot.Item,
                    HostPosition = hostPosition,
                    Amount = amount
                });
            }
        }
    }
}


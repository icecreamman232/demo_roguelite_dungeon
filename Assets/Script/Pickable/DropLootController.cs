using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.HealthSystem;
using SGGames.Script.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SGGames.Script.Pickable
{
    public class DropLootController : MonoBehaviour
    {
        [SerializeField] private float m_spawnRange;
        [SerializeField] private LootTable m_lootTable;
        private Health m_health;
        #if UNITY_EDITOR
        [SerializeField] private bool m_showDebug;
        #endif

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
            
            var pickablePrefabManager = ServiceLocator.GetService<PickablePrefabManager>();

            foreach (var loot in m_lootTable.GetLootTable())
            {
                var amount = Random.Range(loot.MinAmount, loot.MaxAmount);
                
                for (int i = 0; i < amount; i++)
                {
                    var hostPosition = m_health ? m_health.transform.position : transform.position;
                    var spawnPos = GetSpawnPosition(hostPosition);
                    
                    var lootObject = pickablePrefabManager.GetPrefabWith(loot.Item);
                    
                    if (loot.Item == Global.ItemID.Coin)
                    {
                        lootObject.transform.position = spawnPos;
                    }
                    else
                    {
                        Instantiate(lootObject, spawnPos, Quaternion.identity);
                    }
                }
            }
        }

        private Vector2 GetSpawnPosition(Vector2 hostPosition)
        {
            var newPos = Random.insideUnitCircle * m_spawnRange + hostPosition;
            var levelManager = ServiceLocator.GetService<LevelManager>();
            for (int i = 0; i < 30; i++)
            {
                if (levelManager.NormalRoomRect.Contains(newPos))
                {
                    return newPos;
                }
                newPos = Random.insideUnitCircle * m_spawnRange + hostPosition;
            }
            
            //Last try we spawn the loot at monster position which is guaranteed to be in the room area
            return  hostPosition;
        }
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!m_showDebug) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_spawnRange);
        }
        #endif
    }
}


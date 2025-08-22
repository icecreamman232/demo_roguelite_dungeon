using SGGames.Scripts.Core;
using SGGames.Scripts.Data;
using SGGames.Scripts.Events;
using UnityEngine;

namespace SGGames.Scripts.Skills
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] private InventoryItemData m_test;
        [SerializeField] private EquipInventoryItemEvent m_equipItemEvent;
        
        private void Start()
        {
            m_equipItemEvent.AddListener(EquipItem);
        }

        private void OnDestroy()
        {
            m_equipItemEvent.RemoveListener(EquipItem);
        }
        
        private void EquipItem(InventoryItemData data)
        {
            var triggerManager = ServiceLocator.GetService<TriggerManager>();
            triggerManager.AddTrigger(data.TriggerData);

        }
        
        [ContextMenu("Test Equip")]
        private void TestEquip()
        {
            EquipItem(m_test);
        }
    }
}

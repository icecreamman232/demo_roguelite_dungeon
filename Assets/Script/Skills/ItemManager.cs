using System;
using System.Collections;
using System.Collections.Generic;
using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Entity;
using SGGames.Script.Events;
using SGGames.Script.Managers;
using UnityEngine;

namespace SGGames.Script.Skills
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] private InventoryItemData m_test;
        [SerializeField] private TriggerModifierEvent m_triggerModifierEvent;
        [SerializeField] private EquipInventoryItemEvent m_equipItemEvent;
        private PlayerController m_playerController;
        private List<InventoryItemData> m_equippedItemList;
        private Dictionary<InventoryItemData, ushort> m_itemToBeTriggeredDictionary;

        private ushort m_playerEventStatus;
        private ushort m_worldEventStatus;

        private float m_intervalToResetStatus = 2;
        
        private IEnumerator Start()
        {
            var lvlManager = ServiceLocator.GetService<LevelManager>();
            yield return new WaitUntil(() => lvlManager.Player != null);
            m_playerController = lvlManager.Player.GetComponent<PlayerController>();
            
            m_equippedItemList = new List<InventoryItemData>();
            m_itemToBeTriggeredDictionary = new Dictionary<InventoryItemData, ushort>();
            
            RegisterEvents(m_playerController);
            StartCoroutine(OnResetStatus());
        }

        private void OnDestroy()
        {
            UnregisterEvents(m_playerController);
            m_playerController = null;
        }

        private void RegisterEvents(PlayerController playerController)
        {
            m_equipItemEvent.AddListener(EquipItem);
            playerController.PlayerDash.OnDashHitObstacle += OnDashHitObstacle;
            playerController.WeaponHandler.OnAttack += OnAttack;
        }

        private void UnregisterEvents(PlayerController playerController)
        {
            m_equipItemEvent.RemoveListener(EquipItem);
            playerController.PlayerDash.OnDashHitObstacle -= OnDashHitObstacle;
            playerController.WeaponHandler.OnAttack -= OnAttack;
        }

        private IEnumerator OnResetStatus()
        {
            while (true)
            {
                yield return new WaitForSeconds(m_intervalToResetStatus);
                m_playerEventStatus = 0;
                m_worldEventStatus = 0;
                Debug.Log("Reset Status");
            }
        }

        private void OnAttack()
        {
            m_playerEventStatus |= (ushort)Global.PlayerEvents.OnWeaponAttack;
            CheckTriggerCondition();
            Debug.Log($"Event OnAttack {m_playerEventStatus}");
        }

        private void OnDashHitObstacle()
        {
            m_playerEventStatus |= (ushort)Global.PlayerEvents.OnDashHitObstacle;
            CheckTriggerCondition();
            Debug.Log($"Event OnDashHitObstacle {m_playerEventStatus}");
        }

        private void CheckTriggerCondition()
        {
            var itemToRemove = new List<InventoryItemData>();
            foreach (var item in m_itemToBeTriggeredDictionary)
            {
                if (item.Value == m_playerEventStatus)
                {
                    itemToRemove.Add(item.Key);
                    TriggerItem(item.Key);
                }
            }
            foreach (var item in itemToRemove)
            {
                m_itemToBeTriggeredDictionary.Remove(item);
            }
        }

        private void TriggerItem(InventoryItemData data)
        {
            for (int i = 0; i < data.ModifierData.Count; i++)
            {
                m_triggerModifierEvent.Raise(data.ModifierData[i]);
            }
        }
        
        private void EquipItem(InventoryItemData data)
        {
            ushort playerEventStatus = 0;
            foreach (var eventTrigger in data.PlayerEventTrigger)
            {
                playerEventStatus |= (ushort) eventTrigger;
            }
            m_equippedItemList.Add(data);
            if (m_itemToBeTriggeredDictionary == null)
            {
                m_itemToBeTriggeredDictionary = new Dictionary<InventoryItemData, ushort>();
            }
            m_itemToBeTriggeredDictionary.Add(data, playerEventStatus);
        }
        
        [ContextMenu("Test Equip")]
        private void TestEquip()
        {
            EquipItem(m_test);
        }
    }
}

using System;
using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SGGames.Script.Managers
{
    /// <summary>
    /// This class is responsible for dropping items, weapons and currencies upon requesting
    /// </summary>
    public class DropsManager : MonoBehaviour, IGameService
    {
        [Header("Chest Prefabs")]
        [SerializeField] private SpawnChestEvent m_spawnChestEvent;
        [SerializeField] private GameObject m_coinChestPrefab;
        [SerializeField] private GameObject m_keyChestPrefab;
        [SerializeField] private GameObject m_bombChestPrefab;
        [SerializeField] private GameObject m_itemChestPrefab;
        [SerializeField] private GameObject m_weaponChestPrefab;
        [SerializeField] private GameObject m_bossChestPrefab;
        [Header("Item")]
        [SerializeField] private ItemRarityTable m_itemRarityTable;
        [SerializeField] private ItemPickerContainer m_container;
        [SerializeField] private ItemDropsEvent m_itemDropsEvent;
        [Header("Currency")] 
        [SerializeField] private CurrencyDropsEvent m_currencyDropsEvent;
        
        private TreasureChestSelector m_treasureChestSelector;
        private ItemSelector m_itemSelector;
        private const float SPAWN_RANGE_FOR_CURRENCY = 1;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<DropsManager>(this);
            InitializeHelperModules();
            RegisterEvents();
        }

        private void OnDestroy()
        {
            ServiceLocator.UnregisterService<DropsManager>();
            UnregisterEvents();
        }

        private void InitializeHelperModules()
        {
            var itemLibrary = ServiceLocator.GetService<ItemLibrary>();
            var roomManager = ServiceLocator.GetService<RoomManager>();
            
            //Initialize all helper modules
            m_treasureChestSelector = new TreasureChestSelector(
                m_keyChestPrefab, m_coinChestPrefab, m_bombChestPrefab,
                m_itemChestPrefab, m_weaponChestPrefab, m_bossChestPrefab);
            m_itemSelector = new ItemSelector(itemLibrary, roomManager, m_itemRarityTable);
        }

        private void RegisterEvents()
        {
            m_itemDropsEvent.AddListener(OnReceiveItemDropsEvent);
            m_spawnChestEvent.AddListener(OnReceiveSpawnChestEvent);
            m_currencyDropsEvent.AddListener(OnReceiveCurrencyDropsEvent);
        }

        private void UnregisterEvents()
        {
            m_itemDropsEvent.RemoveListener(OnReceiveItemDropsEvent);
            m_spawnChestEvent.RemoveListener(OnReceiveSpawnChestEvent);
            m_currencyDropsEvent.RemoveListener(OnReceiveCurrencyDropsEvent);
        }
        
        private void OnReceiveSpawnChestEvent(Vector3 spawnPosition)
        {
            var roomManager = ServiceLocator.GetService<RoomManager>();
            var chestPrefab = m_treasureChestSelector.GetTreasureChest(roomManager.CurrentRoomReward);
            if(chestPrefab == null) return;
            Instantiate(chestPrefab, spawnPosition, Quaternion.identity);
        }

        private void OnReceiveItemDropsEvent(Vector3 spawnPosition)
        {
            var itemID = m_itemSelector.GetItemForChest().ItemID;
            var itemPrefab = m_container.GetItemPrefabWithID(itemID);
            if (itemPrefab == null)
            {
                Debug.LogError($"Item with id {itemID} not found");
                return;
            }

            Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        }
        
        private void OnReceiveCurrencyDropsEvent(CurrencyDropData currencyDropData)
        {
            if (currencyDropData.ItemID == Global.ItemID.Coin || currencyDropData.ItemID == Global.ItemID.Key || currencyDropData.ItemID == Global.ItemID.Bomb)
            {
                var itemPrefab = m_container.GetCurrencyPrefabWithID(currencyDropData.ItemID);
                if (itemPrefab == null) return;
                Vector3 spawnPosition;
                for (int i = 0; i < currencyDropData.Amount; i++)
                {
                    spawnPosition = currencyDropData.HostPosition + (Vector3)Random.insideUnitCircle * SPAWN_RANGE_FOR_CURRENCY;
                    Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }
}
    

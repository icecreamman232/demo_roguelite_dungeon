using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Managers
{
    /// <summary>
    /// This class is responsible for dropping items, weapons and currencies upon requesting
    /// </summary>
    public class DropsManager : MonoBehaviour, IGameService
    {
        [Header("Chest Prefabs")]
        [SerializeField] private SpawnChestEvent m_spawnChestEvent;
        [SerializeField] private GameObject m_noKeyChestPrefab;
        [SerializeField] private GameObject m_requireKeyChestPrefab;
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

        private void InitializeHelperModules()
        {
            var itemManager = ServiceLocator.GetService<ItemManager>();
            var roomManager = ServiceLocator.GetService<RoomManager>();
            
            //Initialize all helper modules
            m_treasureChestSelector = new TreasureChestSelector(m_noKeyChestPrefab, m_requireKeyChestPrefab, m_bossChestPrefab);
            m_itemSelector = new ItemSelector(itemManager, roomManager, m_itemRarityTable);
        }

        private void RegisterEvents()
        {
            m_itemDropsEvent.AddListener(OnReceiveItemDropsEvent);
            m_spawnChestEvent.AddListener(OnReceiveSpawnChestEvent);
            m_currencyDropsEvent.AddListener(OnReceiveCurrencyDropsEvent);
        }
        
        private void OnReceiveSpawnChestEvent(Vector3 spawnPosition)
        {
            Debug.Log("Spawn chest");
            var roomManager = ServiceLocator.GetService<RoomManager>();
            var chestPrefab = m_treasureChestSelector.GetTreasureChest(roomManager.CurrentRoomReward);
            Instantiate(chestPrefab, spawnPosition, Quaternion.identity);
        }

        private void OnReceiveItemDropsEvent(Vector3 spawnPosition)
        {
            var itemID = m_itemSelector.GetItemForChest().ItemID;
            var itemPrefab = m_container.GetPrefabWithID(itemID);
            if (itemPrefab == null)
            {
                Debug.LogError($"Item with id {itemID} not found");
                return;
            }

            Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        }
        
        private void OnReceiveCurrencyDropsEvent(Global.ItemID itemID, Vector3 hostPosition, int amount)
        {
            if (itemID == Global.ItemID.Coin || itemID == Global.ItemID.Key || itemID == Global.ItemID.Bomb)
            {
                var itemPrefab = m_container.GetPrefabWithID(itemID);
                Vector3 spawnPosition;
                for (int i = 0; i < amount; i++)
                {
                    spawnPosition = hostPosition + (Vector3)Random.insideUnitCircle * SPAWN_RANGE_FOR_CURRENCY;
                    Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }
}
    

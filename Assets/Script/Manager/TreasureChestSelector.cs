using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.Managers
{
    /// <summary>
    /// This class will select proper chest prefab to drop depend on the room reward type
    /// </summary>
    public class TreasureChestSelector
    {
        private GameObject m_keyChestPrefab;
        private GameObject m_coinChestPrefab;
        private GameObject m_bombChestPrefab;
        private GameObject m_itemChestPrefab;
        private GameObject m_weaponChestPrefab;
        private GameObject m_bossChestPrefab;

        private ItemSelector m_itemSelector;

        public TreasureChestSelector(GameObject keyChest, GameObject coinChest, GameObject bombChest,
            GameObject itemChest, GameObject weaponChest, GameObject bossChest)
        {
            m_keyChestPrefab = keyChest;
            m_coinChestPrefab = coinChest;
            m_bombChestPrefab = bombChest;
            m_itemChestPrefab = itemChest;
            m_weaponChestPrefab = weaponChest;
            m_bossChestPrefab = bossChest;
        }
        
        public GameObject GetTreasureChest(Global.RoomRewardType roomRewardType)
        {
            switch (roomRewardType)
            {
                case Global.RoomRewardType.None:
                case Global.RoomRewardType.HealingRoom:
                case Global.RoomRewardType.SacrificeHealthRoom:
                case Global.RoomRewardType.WeaponSmith:
                case Global.RoomRewardType.Random:
                    return null;
                case Global.RoomRewardType.Coin:
                    return m_coinChestPrefab;
                case Global.RoomRewardType.Key:
                    return m_keyChestPrefab;
                case Global.RoomRewardType.Bomb:
                    return m_bombChestPrefab;
                case Global.RoomRewardType.Item:
                    return m_itemChestPrefab;
                case Global.RoomRewardType.Weapon:
                    return m_weaponChestPrefab;
            }
            return null;
        }
    }
}


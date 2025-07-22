using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Managers
{
    /// <summary>
    /// This class will select proper chest prefab to drop depend on the room reward type
    /// </summary>
    public class TreasureChestSelector
    {
        private GameObject m_noKeyChestPrefab;
        private GameObject m_requireKeyChestPrefab;
        private GameObject m_bossChestPrefab;

        private ItemSelector m_itemSelector;

        public TreasureChestSelector(GameObject noKeyChestPrefab, GameObject requireKeyChestPrefab, GameObject bossChestPrefab)
        {
            m_noKeyChestPrefab = noKeyChestPrefab;
            m_requireKeyChestPrefab = requireKeyChestPrefab;
            m_bossChestPrefab = bossChestPrefab;
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
                case Global.RoomRewardType.Key:
                case Global.RoomRewardType.Bomb:
                    return m_noKeyChestPrefab;
                case Global.RoomRewardType.Item:
                case Global.RoomRewardType.Weapon:
                    return m_requireKeyChestPrefab;
            }
            return null;
        }
    }
}


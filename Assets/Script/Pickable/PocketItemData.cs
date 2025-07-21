using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "New Pocket Item Data", menuName = "SGGames/Data/Pocket Item")]
    public class PocketItemData : ItemData
    {
        [SerializeField] private PocketInventoryEvent m_pocketInventoryEvent;
    }
}

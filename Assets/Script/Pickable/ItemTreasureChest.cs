using SGGames.Scripts.Events;
using UnityEngine;

namespace SGGames.Scripts.Pickable
{
    public class ItemTreasureChest : TreasureChest
    {
        [SerializeField] private ItemDropsEvent m_itemDropsEvent;
        protected override void OpenChest()
        {
            m_itemDropsEvent.Raise(transform.position);
            base.OpenChest();
        }
    }
}

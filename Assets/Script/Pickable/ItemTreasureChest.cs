using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Pickable
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

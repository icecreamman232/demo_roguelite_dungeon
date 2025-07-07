using SGGames.Script.Core;
using SGGames.Script.Data;
using UnityEngine;

namespace SGGames.Script.Pickables
{
    public class Pickable : MonoBehaviour
    {
        [SerializeField] protected Global.PickableType m_pickableType;
        [SerializeField] protected ItemData m_itemData;
        [SerializeField] protected int m_amount;
        
        public Global.PickableType PickableType => m_pickableType;
        public ItemData ItemData => m_itemData;
        public int Amount => m_amount;

        /// <summary>
        /// Send item pick data to system that listens to it
        /// </summary>
        protected virtual void PickedUp()
        {
            m_itemData.Picked(m_amount);
        }
    }
}

using System.Collections.Generic;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.UI
{
    public class HealthUIView : UIView
    {
        [SerializeField] private List<HealthUISlot> m_slotList;

        private void Awake()
        {
            m_slotList = new List<HealthUISlot>();
        }

        public void AddSlotToView(HealthUISlot slot)
        {
            m_slotList.Add(slot);
        }

        public void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            var curSlot = currentHealth / Global.HP_PER_SLOT;

            for (int i = 0; i < m_slotList.Count; i++)
            {
                if (i >= curSlot)
                {
                    m_slotList[i].SetSlotType(m_slotList[i].SlotType == Global.HealthSlotType.Health 
                        ? Global.HealthSlotType.HealthDisable 
                        : Global.HealthSlotType.ShieldDisable);
                }
            }
        }
    }
}


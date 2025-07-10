using System.Collections.Generic;
using SGGames.Script.UI;
using UnityEngine;

namespace SGGames.Scripts.UI
{
    public class StaminaUIView : MonoBehaviour
    {
        [SerializeField] private StaminaUISlot m_staminaSlotPrefab;
        private List<StaminaUISlot> m_slotList;
        
        public void SetupView(int max)
        {
            m_slotList = new List<StaminaUISlot>();
            for (int i = 0; i < max; i++)
            {
                var newSlot = Instantiate(m_staminaSlotPrefab, transform);
                m_slotList.Add(newSlot);
            }
        }

        public void UpdateView(int current, int max)
        {
            for (int i = 0; i < max; i++)
            {
                m_slotList[i].UpdateSlot(i < current);
            }
        }
    } 
}


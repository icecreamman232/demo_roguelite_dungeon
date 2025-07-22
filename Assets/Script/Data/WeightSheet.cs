using System;
using UnityEngine;

namespace SGGames.Script.Data
{
    [Serializable]
    public class WeightSheet
    {
        [SerializeField] private string m_id;
        [SerializeField] private WeightValue[] m_weightSheet;
        
        public string Id => m_id;
        public WeightValue GetWeightValue(int index) => m_weightSheet[index];

        public int GetWeightValue(float percentage)
        {
            for (int i = 0; i < m_weightSheet.Length; i++)
            {
                if (m_weightSheet[i].LowerPercentage <= percentage && percentage <= m_weightSheet[i].UpperPercentage)
                {
                    return i;
                }
            }
            return -1;
        }
        
        public void CalculatePercentage()
        {
            var totalWeight = 0f;
            foreach (var weight in m_weightSheet)
            {
                totalWeight += weight.Weight;
            }
            
            var currentPercentage = 0f;
            
            foreach (var weight in m_weightSheet)
            {
                weight.LowerPercentage = currentPercentage;
                weight.UpperPercentage = currentPercentage + weight.Weight / totalWeight * 100f;
                currentPercentage += weight.Weight / totalWeight * 100f;
            }
        }
    }
}
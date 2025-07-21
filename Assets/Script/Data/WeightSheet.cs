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
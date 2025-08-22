using System;
using SGGames.Scripts.EditorExtensions;
using UnityEngine;

namespace SGGames.Scripts.Data
{
    [Serializable]
    public class WeightValue
    {
        [SerializeField] private string m_id;
        [SerializeField] private float m_weight;
        [SerializeField][ReadOnly] private float m_lowerPercentage;
        [SerializeField][ReadOnly] private float m_upperPercentage;
        
        public string Id => m_id;
        public float Weight => m_weight;
        public float LowerPercentage
        {
            get => m_lowerPercentage;
            set => m_lowerPercentage = value;
        }
        public float UpperPercentage
        {
            get => m_upperPercentage;
            set => m_upperPercentage = value;
        }
    }
}
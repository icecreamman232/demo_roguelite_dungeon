using System;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(menuName = "SGGames/Event/Update Player Health", fileName = "New Update Player Health Event")]
    public class UpdatePlayerHealthEvent : ScriptableEvent<UpdatePlayerHealthEventData> { }
    
    [Serializable]
    public class UpdatePlayerHealthEventData
    {
        public float CurrentHealth;
        public float MaxHealth;
        public bool IsInitialize;
    }
}


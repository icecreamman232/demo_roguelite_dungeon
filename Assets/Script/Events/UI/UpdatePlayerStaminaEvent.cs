using System;
using UnityEngine;

namespace SGGames.Scripts.Events
{
    [CreateAssetMenu(menuName = "SGGames/Event/Update Player Stamina", fileName = "New Update Player Stamina Event")]
    public class UpdatePlayerStaminaEvent : ScriptableEvent<UpdatePlayerStaminaEventData> { }
    
    [Serializable]
    public class UpdatePlayerStaminaEventData
    {
        public int CurrentStamina;
        public int MaxStamina;
        public bool IsInitialize;
    }
}

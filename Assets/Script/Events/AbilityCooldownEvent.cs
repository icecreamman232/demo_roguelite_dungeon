using System;
using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.Events
{
    [CreateAssetMenu(fileName = "Ability Cooldown Event", menuName = "SGGames/Event/Ability Cooldown")]
    public class AbilityCooldownEvent : ScriptableEvent<AbilityCooldownEventData> { }

    [Serializable]
    public class AbilityCooldownEventData
    {
        public Global.AbilityType AbilityType;
        public int Cooldown;
    }
}

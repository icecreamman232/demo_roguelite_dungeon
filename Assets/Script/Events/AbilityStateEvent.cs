using System;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(fileName = "Ability State Event", menuName = "SGGames/Event/Ability State")]
    public class AbilityStateEvent : ScriptableEvent<AbilityStateEventData> { }

    [Serializable]
    public class AbilityStateEventData
    {
        public Global.AbilityState AbilityState;
        public Global.AbilityID AbilityID;
    }
}

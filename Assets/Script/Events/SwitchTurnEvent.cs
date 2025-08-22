using System;
using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.Events
{
    /// <summary>
    /// Event for switch turn.
    /// </summary>
    [CreateAssetMenu(fileName = "Switch Turn Event", menuName = "SGGames/Event/Switch Turn")]
    public class SwitchTurnEvent : ScriptableEvent<TurnBaseEventData> { }

    [Serializable]
    public class TurnBaseEventData
    {
        public Global.TurnBaseState TurnBaseState;
        public int EntityIndex;
    }
}

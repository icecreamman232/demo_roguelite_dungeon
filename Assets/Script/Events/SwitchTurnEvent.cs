using System;
using SGGames.Script.Core;

using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(fileName = "Switch Turn Event", menuName = "SGGames/Event/Switch Turn")]
    public class SwitchTurnEvent : ScriptableEvent<TurnBaseEventData> { }

    [Serializable]
    public class TurnBaseEventData
    {
        public Global.TurnBaseState TurnBaseState;
        public int EntityIndex;
    }
}

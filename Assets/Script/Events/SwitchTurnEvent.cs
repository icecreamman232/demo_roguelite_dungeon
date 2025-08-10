using SGGames.Script.Core;

using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(fileName = "Switch Turn Event", menuName = "SGGames/Event/Switch Turn")]
    public class SwitchTurnEvent : ScriptableEvent<Global.TurnBaseType> { }
}

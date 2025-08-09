using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(menuName = "SGGames/Event/Game Event", fileName = "Game Event")]
    public class GameEvent : ScriptableEvent<Global.GameEventType> { }
}
using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.Events
{
    [CreateAssetMenu(menuName = "SGGames/Event/Game Event", fileName = "Game Event")]
    public class GameEvent : ScriptableEvent<Global.GameEventType> { }
}
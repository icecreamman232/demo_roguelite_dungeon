using UnityEngine;

namespace SGGames.Scripts.Events
{
    [CreateAssetMenu(fileName = "Player Use Action Point Event", menuName = "SGGames/Event/Player Use Action Point")]
    public class PlayerUseActionPointEvent : ScriptableEvent<int> { }
}

using UnityEngine;

namespace SGGames.Scripts.Events
{
    [CreateAssetMenu(fileName ="Item Drops Event", menuName = "SGGames/Event/Item Drops ")]
    public class ItemDropsEvent : ScriptableEvent<Vector3> { }
}

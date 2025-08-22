using UnityEngine;

namespace SGGames.Scripts.Events
{
    /// <summary>
    /// Event for spawning a chest.
    /// </summary>
    [CreateAssetMenu(menuName = "SGGames/Event/Spawn Chest", fileName = "Spawn Chest Event")]
    public class SpawnChestEvent : ScriptableEvent<Vector3> { }
}

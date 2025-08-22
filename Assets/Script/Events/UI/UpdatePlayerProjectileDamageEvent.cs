using UnityEngine;

namespace SGGames.Scripts.Events
{
    [CreateAssetMenu(menuName = "SGGames/Event/Update Player Dmg Proj", fileName = "New Update Player Dmg Proj Event")]
    public class UpdatePlayerProjectileDamageEvent : ScriptableEvent<float> { }
}
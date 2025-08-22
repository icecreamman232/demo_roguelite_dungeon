using UnityEngine;

namespace SGGames.Scripts.Events
{
    [CreateAssetMenu(menuName = "SGGames/Event/Update Currency", fileName = "New Update Currency Event")]
    public class UpdateCurrencyUIEvent : ScriptableEvent<CurrencyUpdateData> { }
}
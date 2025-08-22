using UnityEngine;

namespace SGGames.Scripts.Items
{
    [CreateAssetMenu(fileName = "Damage Resistance Modifier Data", menuName = "SGGames/Modifier/Damage Resist")]
    public class DamageResistanceModifierData : ModifierData
    {
        [SerializeField] private float m_addingDmgResistance;
        public float AddingDmgResistance => m_addingDmgResistance;
    }
}

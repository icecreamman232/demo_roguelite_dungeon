using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Pickable
{
    [CreateAssetMenu(fileName = "Increase Player Size Item Effect", menuName = "SGGames/Item Effect/Increase Flat Dmg")]
    public class IncreasePlayerFlatDamageItemEffect : ItemEffect
    {
        [SerializeField] private float m_damageIncrease;
        [SerializeField] private UpdatePlayerProjectileDamageEvent m_playerProjectileDamageEvent;
        
        public override void ApplyEffect(GameObject target)
        {
            m_playerProjectileDamageEvent.Raise(m_damageIncrease);
        }

        public override void RemoveEffect(GameObject target)
        {
            m_playerProjectileDamageEvent.Raise(-m_damageIncrease);
        }
    }
}


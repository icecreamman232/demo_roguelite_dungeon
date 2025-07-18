using SGGames.Script.Entity;
using UnityEngine;

namespace SGGames.Script.Pickable
{
    [CreateAssetMenu(fileName = "Increase Player Speed Item Effect", menuName = "SGGames/Item Effect/Increase Flat Speed")]
    public class IncreaseFlatPlayerSpeedItemEffect : ItemEffect
    {
        [SerializeField] private float m_speedIncrease;
        
        public override void ApplyEffect(GameObject target)
        {
            var controller = target.GetComponent<PlayerController>();
            controller.PlayerMovement.AddFlatSpeedBonus(m_speedIncrease);
        }

        public override void RemoveEffect(GameObject target)
        {
            var controller = target.GetComponent<PlayerController>();
            controller.PlayerMovement.AddFlatSpeedBonus(-m_speedIncrease);
        }
    }

}

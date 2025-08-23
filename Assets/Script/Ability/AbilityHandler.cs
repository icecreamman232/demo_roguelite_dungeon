using System.Collections.Generic;
using UnityEngine;

namespace SGGames.Scripts.Abilities
{
    public class AbilityHandler : MonoBehaviour
    {
        [SerializeField] private List<AbilityBehavior> m_abilities;

        private void Awake()
        {
            foreach(var ability in m_abilities)
            {
                if (ability.IsPermit && ability.IsDefaultActivated)
                {
                    ability.Activate();
                }
            }
        }

        public void AddAbility(AbilityBehavior ability)
        {
            var newAbility = Instantiate(ability,transform);
            m_abilities.Add(newAbility);
            if (newAbility.IsPermit && newAbility.IsDefaultActivated)
            {
                newAbility.Activate();
            }
        }
    }
}

using UnityEngine;

namespace SGGames.Scripts.Abilities
{
    public class AbilityHandler : MonoBehaviour
    {
        [SerializeField] private AbilityBehavior[] m_abilities;

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
    }
}

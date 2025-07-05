using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(menuName = "SGGames/Data/Weapon",fileName = "New Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private float m_coolDown;
        
        
        public float Cooldown => m_coolDown;
    }
}

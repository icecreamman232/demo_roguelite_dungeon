using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Pickable
{
    public abstract class ItemEffect : ScriptableObject
    {
        [SerializeField] private Global.ItemEffectID m_itemEffectID;
        [SerializeField] private Global.ItemEffectTag m_itemEffectTag;
        
        public Global.ItemEffectID ItemEffectID => m_itemEffectID;
        public Global.ItemEffectTag ItemEffectTag => m_itemEffectTag;
        
        public abstract void ApplyEffect(GameObject target);
        public abstract void RemoveEffect(GameObject target);
    }
}

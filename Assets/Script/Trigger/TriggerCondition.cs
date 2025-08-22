using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.Items
{
    public abstract class TriggerCondition : ScriptableObject
    {
        public abstract bool Evaluate(Global.WorldEventType evenType, GameObject source, GameObject target);
    }
}

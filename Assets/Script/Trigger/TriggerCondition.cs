using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Items
{
    public abstract class TriggerCondition : ScriptableObject
    {
        public abstract bool Evaluate(Global.WorldEventType evenType, GameObject source, GameObject target);
    }
}

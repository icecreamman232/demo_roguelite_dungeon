using UnityEngine;

namespace SGGames.Script.Items
{
    public abstract class TriggerAction : ScriptableObject
    {
        public abstract void Execute(GameObject source, GameObject target);
    }
}

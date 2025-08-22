using UnityEngine;

namespace SGGames.Scripts.Items
{
    public abstract class TriggerAction : ScriptableObject
    {
        public abstract void Execute(GameObject source, GameObject target);
    }
}

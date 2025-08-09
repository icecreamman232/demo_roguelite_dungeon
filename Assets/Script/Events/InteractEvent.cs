using System;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(fileName = "Interact Event", menuName = "SGGames/Event/Interact Event")]
    public class InteractEvent : ScriptableEvent<InteractEventData> { }

    [Serializable]
    public class InteractEventData
    {
        public Global.InteractEventType InteractEventType;
        public int Layer;
        public string Tag;
    }
}


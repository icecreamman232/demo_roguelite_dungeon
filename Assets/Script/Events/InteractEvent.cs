using System;
using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.Events
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


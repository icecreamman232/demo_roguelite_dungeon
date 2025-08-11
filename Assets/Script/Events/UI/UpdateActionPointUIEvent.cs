using System;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(fileName = "Update Action Point UI Event", menuName = "SGGames/Event/Update Action Point UI ")]
    public class UpdateActionPointUIEvent : ScriptableEvent<ActionPointUIData> { }

    [Serializable]
    public class ActionPointUIData
    {
        public int CurrentActionPoint;
        public int MaxActionPoint;
    }
}

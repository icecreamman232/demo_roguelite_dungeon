using System;
using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.Events
{
    [CreateAssetMenu(fileName = "Hud Button Event", menuName = "SGGames/Event/Hud Button")]
    public class HudButtonEvent : ScriptableEvent<HudButtonEventData> { }

    [Serializable]
    public class HudButtonEventData
    {
        public Global.HudButtonType HudButtonType;
    }
}

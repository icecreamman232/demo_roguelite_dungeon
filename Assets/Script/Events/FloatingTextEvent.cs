using System;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "Floating Text Event", menuName = "SGGames/Event/Floating Text")]
    public class FloatingTextEvent: ScriptableEvent<FloatingTextData> { }

    [Serializable]
    public class FloatingTextData
    {
        public string Content;
        public Vector3 Position;
    }
}
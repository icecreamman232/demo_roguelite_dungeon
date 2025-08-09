using System;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(fileName = "Currency Drops Event", menuName = "SGGames/Event/Currency Drops ")]
    public class CurrencyDropsEvent : ScriptableEvent<CurrencyDropData> { }

    [Serializable]
    public class CurrencyDropData
    {
        public Global.ItemID ItemID;
        public Vector3 HostPosition;
        public int Amount;
    }
}

using System;
using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.Events
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

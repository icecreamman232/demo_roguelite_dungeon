using System;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(fileName = "Currency Event", menuName = "SGGames/Event/Currency")]
    public class CurrencyEvent : ScriptableEvent<CurrencyUpdateData> { }

    [Serializable]
    public class CurrencyUpdateData
    {
        public Global.ItemID ItemID;
        public int Amount;
    }
}

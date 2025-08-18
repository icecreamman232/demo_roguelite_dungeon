using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(fileName = "Display Effect Tile Event", menuName = "SGGames/Event/Display Effect Tile")]
    public class DisplayEffectTileEvent : ScriptableEvent<EffectTileEventData> { }


    public class EffectTileEventData
    {
        public Vector3 Position;
        public Global.EffectTileType EffectTileType;
    }
}

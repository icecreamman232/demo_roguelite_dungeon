using System.Collections.Generic;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(fileName = "Display Effect Tile Event", menuName = "SGGames/Event/Display Effect Tile")]
    public class DisplayEffectTileEvent : ScriptableEvent<EffectTileEventData> { }
    
    public class EffectTileEventData
    {
        public List<Vector3> Position = new List<Vector3>();
        public Global.EffectTileType EffectTileType;

        public void SetData(List<Vector3> position, Global.EffectTileType effectTileType)
        {
            Position = position;
            EffectTileType = effectTileType;
        }
    }
}

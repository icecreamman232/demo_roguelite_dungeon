using System.Collections.Generic;
using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.Events
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

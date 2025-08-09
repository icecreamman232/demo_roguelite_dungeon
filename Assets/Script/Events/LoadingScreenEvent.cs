using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(fileName = "Loading Screen Event", menuName = "SGGames/Event/Loading Screen")]
    public class LoadingScreenEvent : ScriptableEvent<Global.LoadingScreenType> { }
}

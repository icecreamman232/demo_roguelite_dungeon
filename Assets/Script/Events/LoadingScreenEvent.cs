using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.Events
{
    [CreateAssetMenu(fileName = "Loading Screen Event", menuName = "SGGames/Event/Loading Screen")]
    public class LoadingScreenEvent : ScriptableEvent<Global.LoadingScreenType> { }
}

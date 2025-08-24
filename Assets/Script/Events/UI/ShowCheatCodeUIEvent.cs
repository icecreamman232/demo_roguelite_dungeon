using SGGames.Scripts.Events;
using UnityEngine;

namespace SGGames.Scrips.Events
{
    [CreateAssetMenu(fileName = "Show Cheat Code UI Event", menuName = "SGGames/Event/Show Cheat Code UI")]
    public class ShowCheatCodeUIEvent : ScriptableEvent<CheatCodeUIData> { }

    public class CheatCodeUIData
    {
        public string RoomName;
    }
}

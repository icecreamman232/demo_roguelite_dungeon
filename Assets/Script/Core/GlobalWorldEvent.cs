using UnityEngine;

namespace SGGames.Script.Core
{
    public static partial class Global
    {
        public enum WorldEventType
        {
            //ROOM
            OnEnterNewRoom = 0,
            OnRoomCleared,
            
            //PlAYER
            OnPlayerStartDash = 300,
            OnPlayerDashCanceled, //For when player dash into obstacles
            OnPlayerStopDash,
            
            //ENEMIES
            OnEnemyDeath = 900,
        }
    }
}

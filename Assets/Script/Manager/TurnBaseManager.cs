using SGGames.Script.Core;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Managers
{
    public class TurnBaseManager : MonoBehaviour, IGameService
    {
        /// <summary>
        /// Indicate which entity is in this turn
        /// </summary>
        [SerializeField] private Global.TurnBaseType m_turnBaseType;
        [SerializeField] private SwitchTurnEvent m_switchTurnEvent;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<TurnBaseManager>(this);    
        }

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            m_switchTurnEvent.RemoveListener(OnSwitchTurn);
        }

        private void Initialize()
        {
            m_turnBaseType = Global.TurnBaseType.Player;
            m_switchTurnEvent.AddListener(OnSwitchTurn);
        }

        private void SwitchToPlayerTurn()
        {
            
        }

        private void SwitchToEnemyTurn()
        {
            
        }

        private void OnSwitchTurn(Global.TurnBaseType nextTurnType)
        {
            if (nextTurnType == Global.TurnBaseType.Player)
            {
                SwitchToPlayerTurn();
            }
            else
            {
                SwitchToEnemyTurn();
            }
        }
    }
}

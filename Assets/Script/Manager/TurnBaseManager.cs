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
            InitializeInternal();
        }

        private void Start()
        {
            InitializeExternal();
        }

        private void OnDestroy()
        {
            m_switchTurnEvent.RemoveListener(OnSwitchTurn);
        }

        private void InitializeInternal()
        {
            m_turnBaseType = Global.TurnBaseType.Player;
            m_switchTurnEvent.AddListener(OnSwitchTurn);
        }

        private void InitializeExternal()
        {
            ServiceLocator.RegisterService<TurnBaseManager>(this);  
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

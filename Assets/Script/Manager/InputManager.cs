using System;
using SGGames.Script.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SGGames.Script.Managers
{
    /// <summary>
    /// Manager class for all input in game
    /// </summary>
    public class InputManager : MonoBehaviour, IGameService
    {
        [SerializeField] private bool m_isAllowInput;

        private Vector2 m_movementInput;
        private InputAction m_moveAction;
        
        public bool IsAllowInput => m_isAllowInput;
        public Action<Vector2> OnMoveInputUpdate;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<InputManager>(this);
            m_moveAction = InputSystem.actions.FindAction("Move");
            
            EnableInput();
        }

        private void Update()
        {
            if (!m_isAllowInput) return;

            m_movementInput = m_moveAction.ReadValue<Vector2>();
            OnMoveInputUpdate?.Invoke(m_movementInput);
        }

        
        /// <summary>
        /// Enable player input
        /// </summary>
        public void EnableInput()
        {
            m_isAllowInput = true;
            InputSystem.actions.Enable();
        }

        
        /// <summary>
        /// Disable player input
        /// </summary>
        public void DisableInput()
        {
            m_isAllowInput = false;
            InputSystem.actions.Disable();
        }
        
        private void OnDestroy()
        {
            ServiceLocator.UnregisterService<InputManager>();
        }
    }
}


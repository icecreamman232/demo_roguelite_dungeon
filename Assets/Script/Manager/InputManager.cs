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

        private Camera m_camera;
        private Vector2 m_movementInput;
        private Vector3 m_worldMousePosition;
        private InputAction m_moveAction;
        private InputAction m_attackAction;
        private InputAction m_dashAction;
        
        public bool IsAllowInput => m_isAllowInput;
        public Action<Vector2> OnMoveInputUpdate;
        public Action<Vector3> WorldMousePositionUpdate;
        public Action OnPressAttack;
        public Action OnPressDash;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<InputManager>(this);
            m_moveAction = InputSystem.actions.FindAction("Move");
            m_attackAction = InputSystem.actions.FindAction("Attack");
            m_dashAction = InputSystem.actions.FindAction("Dash");
            m_attackAction.performed += OnAttackButtonPressed;
            m_dashAction.performed += OnDashButtonPressed;
            m_camera = Camera.main;
            EnableInput();
        }

        private void Update()
        {
            if (!m_isAllowInput) return;

            m_movementInput = m_moveAction.ReadValue<Vector2>();
            OnMoveInputUpdate?.Invoke(m_movementInput);

            m_worldMousePosition = ComputeWorldMousePosition();
            WorldMousePositionUpdate?.Invoke(m_worldMousePosition);
        }
        
        private Vector3 ComputeWorldMousePosition()
        {
            var mousePos = Input.mousePosition;
            mousePos = m_camera.ScreenToWorldPoint(mousePos);
            mousePos.z = 0;
            return mousePos;    
        }

        private void OnAttackButtonPressed(InputAction.CallbackContext context)
        {
            if (!m_isAllowInput) return;
            OnPressAttack?.Invoke();
        }
        
        private void OnDashButtonPressed(InputAction.CallbackContext context)
        {
            if (!m_isAllowInput) return;
            OnPressDash?.Invoke();
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
    }
}


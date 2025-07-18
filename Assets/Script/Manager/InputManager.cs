using System;
using SGGames.Script.Core;
using SGGames.Script.UI;
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
        [SerializeField] private bool m_isAllowGameplayInput;

        private Camera m_camera;
        private Vector2 m_movementInput;
        private Vector3 m_worldMousePosition;
        private InputAction m_moveAction;
        private InputAction m_attackAction;
        private InputAction m_dashAction;
        private InputAction m_openConsole;
        private InputAction m_closeUI;
        private InputAction m_interactAction;
        
        public bool IsAllowInput => m_isAllowInput;
        public Action<Vector2> OnMoveInputUpdate;
        public Action<Vector3> WorldMousePositionUpdate;
        public Action OnPressAttack;
        public Action OnPressDash;
        public Action OnPressOpenConsole;
        public Action OnPressCloseUI;
        public Action OnPressInteract;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<InputManager>(this);
            m_moveAction = InputSystem.actions.FindAction("Move");
            m_attackAction = InputSystem.actions.FindAction("Attack");
            m_dashAction = InputSystem.actions.FindAction("Dash");
            m_openConsole = InputSystem.actions.FindAction("Open Console");
            m_closeUI = InputSystem.actions.FindAction("Close UI");
            m_interactAction = InputSystem.actions.FindAction("Interact");
            
            m_attackAction.performed += OnAttackButtonPressed;
            m_dashAction.performed += OnDashButtonPressed;
            m_openConsole.performed += OnOpenConsoleButtonPressed;
            m_closeUI.performed += OnCloseUIButtonPressed;
            m_interactAction.performed += OnInteractButtonPressed;
            
            m_camera = Camera.main;
            EnableInput();
        }

        private void Update()
        {
            if (!m_isAllowInput) return;
            if (!m_isAllowGameplayInput) return;

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
            if (!m_isAllowGameplayInput) return;
            OnPressAttack?.Invoke();
        }
        
        private void OnDashButtonPressed(InputAction.CallbackContext context)
        {
            if (!m_isAllowInput) return;
            if (!m_isAllowGameplayInput) return;
            OnPressDash?.Invoke();
        }
        
        private void OnOpenConsoleButtonPressed(InputAction.CallbackContext context)
        {
            if (!m_isAllowInput) return;
            if (ConsoleCanvasController.IsConsoleOpen) return;
            OnPressOpenConsole?.Invoke();
        }
        
        private void OnCloseUIButtonPressed(InputAction.CallbackContext context)
        {
            if (!m_isAllowInput) return;
            
            OnPressCloseUI?.Invoke();
        }
        
        private void OnInteractButtonPressed(InputAction.CallbackContext context)
        {
            if (!m_isAllowInput) return;
            if (ConsoleCanvasController.IsConsoleOpen) return;
            OnPressInteract?.Invoke();
        }
        
        /// <summary>
        /// Enable player input
        /// </summary>
        public void EnableInput()
        {
            m_isAllowInput = true;
            m_isAllowGameplayInput = true;
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

        public void DisableGameplayInput()
        {
            m_isAllowGameplayInput = false;
        }
    }
}


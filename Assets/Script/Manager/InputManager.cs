using System;
using SGGames.Scripts.UI;
using SGGames.Scripts.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace SGGames.Scripts.Managers
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
        private InputAction m_specialAbilityAction;
        private InputAction m_executeAction;
        private InputAction m_openConsole;
        private InputAction m_closeUI;
        private InputAction m_interactAction;
        private InputAction m_endTurnAction;
        private InputAction m_cancelAction;
        
        public bool IsAllowInput => m_isAllowInput;
        public Action<Vector2> OnMoveInputUpdate;
        public Action<Vector3> WorldMousePositionUpdate;
        public Action OnPressAttack;
        public Action OnPressSpecialAbility;
        public Action OnPressExecute;
        public Action OnPressOpenConsole;
        public Action OnPressCloseUI;
        public Action OnPressInteract;
        public Action OnPressEndTurn;
        public Action OnCancel;
        private float m_attackCooldownTimer;
        private bool m_isAttacking;
        private float m_attackCooldownTime = 0.1f;

        #region Unity Methods
        
        private void Awake()
        {
            ServiceLocator.RegisterService<InputManager>(this);
            AssignActions();
            m_camera = Camera.main;
            EnableInput();
        }

        private void OnDestroy()
        {
            UnassignActions();
        }

        private void Update()
        {
            if (!m_isAllowInput) return;
            if (!m_isAllowGameplayInput) return;

            // Update attack cooldown timer
            if (m_attackCooldownTimer > 0)
            {
                m_attackCooldownTimer -= Time.deltaTime;
            }

            m_worldMousePosition = ComputeWorldMousePosition();
            WorldMousePositionUpdate?.Invoke(m_worldMousePosition);
        }

        
        #endregion
        
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

        private void AssignActions()
        {
            m_moveAction = InputSystem.actions.FindAction("Move");
            m_attackAction = InputSystem.actions.FindAction("Attack");
            m_specialAbilityAction = InputSystem.actions.FindAction("Special Ability");
            m_executeAction = InputSystem.actions.FindAction("Execute");
            m_openConsole = InputSystem.actions.FindAction("Open Console");
            m_closeUI = InputSystem.actions.FindAction("Close UI");
            m_interactAction = InputSystem.actions.FindAction("Interact");
            m_endTurnAction = InputSystem.actions.FindAction("End Turn");
            m_cancelAction = InputSystem.actions.FindAction("Cancel");
            

            m_moveAction.performed += OnMoveInputPressed;
            m_attackAction.performed += OnAttackButtonPressed;
            m_attackAction.canceled += OnAttackButtonReleased;
            m_specialAbilityAction.performed += OnSpecialAbilityButtonPressed;
            m_executeAction.performed += OnExecuteButtonPressed;
            m_openConsole.performed += OnOpenConsoleButtonPressed;
            m_closeUI.performed += OnCloseUIButtonPressed;
            m_interactAction.performed += OnInteractButtonPressed;
            m_endTurnAction.performed += OnEndTurnButtonPressed;
            m_cancelAction.performed += OnCancelButtonPressed;
        }
        

        private void UnassignActions()
        {
            m_moveAction.performed -= OnMoveInputPressed;
            m_attackAction.performed -= OnAttackButtonPressed;
            m_attackAction.canceled -= OnAttackButtonReleased;
            m_specialAbilityAction.performed -= OnSpecialAbilityButtonPressed;
            m_executeAction.performed -= OnExecuteButtonPressed;
            m_openConsole.performed -= OnOpenConsoleButtonPressed;
            m_closeUI.performed -= OnCloseUIButtonPressed;
            m_interactAction.performed -= OnInteractButtonPressed;
            m_endTurnAction.performed -= OnEndTurnButtonPressed;
            m_cancelAction.performed -= OnCancelButtonPressed;
        }

        /// <summary>
        /// Check if attack has priority over movement
        /// </summary>
        private bool ShouldBlockMovement()
        {
            return m_isAttacking || m_attackCooldownTimer > 0;
        }
        
        #region Callback for Buttons
        
        private Vector3 ComputeWorldMousePosition()
        {
            var mousePos = Input.mousePosition;
            mousePos = m_camera.ScreenToWorldPoint(mousePos);
            mousePos.z = 0;
            return mousePos;    
        }
        
        private void OnExecuteButtonPressed(InputAction.CallbackContext context)
        {
            if (!m_isAllowInput) return;
            if (!m_isAllowGameplayInput) return;
            OnPressExecute?.Invoke();
        }
        
        private void OnMoveInputPressed(InputAction.CallbackContext context)
        {
            if (!m_isAllowInput) return;
            if (!m_isAllowGameplayInput) return;
            if (ShouldBlockMovement()) return;
            m_movementInput = context.ReadValue<Vector2>();
            OnMoveInputUpdate?.Invoke(m_movementInput);
        }

        private void OnAttackButtonPressed(InputAction.CallbackContext context)
        {
            if (!m_isAllowInput) return;
            if (!m_isAllowGameplayInput) return;
            if (EventSystem.current.currentSelectedGameObject != null) return;
            OnPressAttack?.Invoke();
            m_isAttacking = true;
            m_attackCooldownTimer = m_attackCooldownTime;
            // Stop movement when attacking
            m_movementInput = Vector2.zero;
            OnMoveInputUpdate?.Invoke(m_movementInput);
        }
        
        private void OnAttackButtonReleased(InputAction.CallbackContext context)
        {
            if (!m_isAllowInput) return;
            if (!m_isAllowGameplayInput) return;
            m_isAttacking = false;
        }
        
        private void OnSpecialAbilityButtonPressed(InputAction.CallbackContext context)
        {
            if (!m_isAllowInput) return;
            if (!m_isAllowGameplayInput) return;
            OnPressSpecialAbility?.Invoke();
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
        
        private void OnEndTurnButtonPressed(InputAction.CallbackContext context)
        {
            if (!m_isAllowInput) return;
            if (!m_isAllowGameplayInput) return;
            OnPressEndTurn?.Invoke();
        }

        
        private void OnCancelButtonPressed(InputAction.CallbackContext context)
        {
            if (!m_isAllowInput) return;
            if (!m_isAllowGameplayInput) return;
            OnCancel?.Invoke();
        }
        
        #endregion
    }
}


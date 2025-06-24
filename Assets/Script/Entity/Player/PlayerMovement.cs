using System;
using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Managers;
using UnityEngine;

namespace SGGames.Script.Entity
{
    public class PlayerMovement : EntityMovement
    {
        [Header("Player")]
        [SerializeField] private PlayerData m_playerData;
        
        private void Start()
        {
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.OnMoveInputUpdate += UpdateMoveInput;
            
            m_moveSpeed = m_playerData.MoveSpeed;
        }

        private void UpdateMoveInput(Vector2 moveInput)
        {
            m_movementDirection = moveInput;
        }

        private void OnDestroy()
        {
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.OnMoveInputUpdate -= UpdateMoveInput;
        }
    }
}

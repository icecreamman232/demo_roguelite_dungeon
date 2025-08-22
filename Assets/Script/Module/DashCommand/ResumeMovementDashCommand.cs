using SGGames.Scripts.Entities;
using UnityEngine;

namespace SGGames.Scripts.Modules
{
    public class ResumeMovementDashCommand : IDashCommand
    {
        private PlayerMovement m_movement;
        
        public void Initialize(GameObject target)
        {
            m_movement = target.GetComponent<PlayerMovement>();
        }

        public void Execute()
        {
            m_movement.ResumeMovement();
        }
    }
}
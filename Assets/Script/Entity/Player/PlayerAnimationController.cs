using UnityEngine;

namespace SGGames.Scripts.Entities
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Transform m_model;
        [SerializeField] private Animator m_animator;
        
        private readonly int CANT_MOVE_ANIMATION = Animator.StringToHash("CantMove");
        
        public void PlayCantMoveAnimation()
        {
            m_animator.SetTrigger(CANT_MOVE_ANIMATION);
        }
    }
}

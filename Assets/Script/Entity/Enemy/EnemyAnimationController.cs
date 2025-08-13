using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Entity
{
    public class EnemyAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator m_animator;
        private readonly int BODY_SLAM_ANIMATION = Animator.StringToHash("Attack");
        private readonly int BODY_SLAM_DIRECTION_PARAM = Animator.StringToHash("AttackDirection");
        
        public void PlayBodySlamAnimation(Global.Direction attackDirection)
        {
            Debug.Log($"Attack Direction: {attackDirection}");
            m_animator.SetInteger(BODY_SLAM_DIRECTION_PARAM, (int)attackDirection);
            m_animator.SetTrigger(BODY_SLAM_ANIMATION);
        }
    }
}

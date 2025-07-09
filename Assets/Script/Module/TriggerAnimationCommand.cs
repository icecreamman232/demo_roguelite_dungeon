using System.Collections;
using UnityEngine;

namespace SGGames.Script.Modules
{
    [CreateAssetMenu(fileName = "Play Animation Command",menuName = "SGGames/Command/Trigger Animation")]
    public class TriggerAnimationCommand : ICommand
    {
        [SerializeField] private string m_animationParam;
        
        public override IEnumerator Execute(GameObject target)
        {
            var animator = target.GetComponentInChildren<Animator>();
            if (m_delay > 0f)
            {
                yield return new WaitForSeconds(m_delay);
            }
  
            animator.SetTrigger(m_animationParam);
        }
    }

}

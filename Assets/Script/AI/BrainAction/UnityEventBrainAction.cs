using SGGames.Scripts.AI;
using UnityEngine;
using UnityEngine.Events;

namespace SGGames.Script.AI
{
    public class UnityEventBrainAction : BrainAction
    {
        [SerializeField] private UnityEvent m_event;
        
        public override void DoAction()
        {
            m_event?.Invoke();
        }
    }
}


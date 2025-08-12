using SGGames.Script.Core;
using SGGames.Scripts.AI;
using UnityEngine;
using UnityEngine.Events;

namespace SGGames.Script.AI
{
    public class UnityEventBrainAction : BrainAction
    {
        [SerializeField] private UnityEvent m_event;

        public override void StartTurnAction()
        {
            m_event?.Invoke();
            SetActionState(Global.ActionState.Completed);
        }
    }
}


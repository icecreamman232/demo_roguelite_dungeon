using SGGames.Scripts.AI;
using UnityEngine;

namespace SGGames.Script.AI
{
    public class RandomMoveBrainAction : BrainAction
    {
        public override void StartTurnAction()
        {
            var randomX = Random.Range(-1.0f, 1.0f);
            var randomY = Random.Range(-1.0f, 1.0f);
            m_brain.Owner.Movement.SetDirection((new Vector2(randomX, randomY)).normalized);
            SetActionState(Core.Global.ActionState.InProgress);
        }
    } 
}


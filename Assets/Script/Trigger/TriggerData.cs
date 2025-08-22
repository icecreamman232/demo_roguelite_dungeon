using SGGames.Scripts.Core;
using SGGames.Scripts.EditorExtensions;
using UnityEngine;

namespace SGGames.Scripts.Items
{
    [CreateAssetMenu(fileName = "Trigger Data", menuName = "SGGames/Trigger/Trigger Data")]
    public class TriggerData : ScriptableObject
    {
        [SerializeField] private Global.WorldEventType m_evenTrigger;
        [SerializeField] [ShowProperties] private TriggerCondition[] m_conditions;
        [SerializeField] [ShowProperties] private TriggerAction[] m_actions;

        public bool CheckEvent(Global.WorldEventType eventType)
        {
            return m_evenTrigger == eventType;
        }

        public void Execute(Global.WorldEventType eventType, GameObject source, GameObject target)
        {
            foreach (var condition in m_conditions)
            {
                if (!condition.Evaluate(eventType, source, target)) return;
            }

            foreach (var action in m_actions)
            {
                action.Execute(source, target);
            }
        }
    } 
}


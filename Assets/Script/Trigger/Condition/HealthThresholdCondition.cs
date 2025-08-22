using SGGames.Scripts.Entities;
using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.Items
{
    [CreateAssetMenu(fileName = "Health Threshold Condition", menuName = "SGGames/Trigger/Condition/Health Threshold")]
    public class HealthThresholdCondition : TriggerCondition
    {
        [SerializeField] private Global.ComparisonType m_comparisonType;
        [SerializeField] private bool m_isFlatValue;
        [SerializeField] private float m_healthThreshold;
    
        public override bool Evaluate(Global.WorldEventType evenType, GameObject source, GameObject target)
        {
            var identifier = source.GetComponent<IEntityIdentifier>();
            if (identifier.IsPlayer())
            {
                var playerHealth = ((PlayerController)identifier).Health;
                if (m_isFlatValue)
                {
                    return Global.ComparisionBetweenFloat(leftVal: playerHealth.CurrentHealth, rightVal: m_healthThreshold, m_comparisonType);
                }
                else
                {
                    var healthAmountToCompare = playerHealth.MaxHealth * m_healthThreshold/100;
                    return Global.ComparisionBetweenFloat(leftVal: playerHealth.CurrentHealth, rightVal: healthAmountToCompare, m_comparisonType);   
                }
            }
            return false;
        }
    }
}


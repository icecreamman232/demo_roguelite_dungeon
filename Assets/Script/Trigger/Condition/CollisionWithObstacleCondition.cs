using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.Items
{
    [CreateAssetMenu(fileName = "Collision With Obstacle Condition", menuName = "SGGames/Trigger/Condition/Collision With Obstacle")]
    public class CollisionWithObstacleCondition : TriggerCondition
    {
        [SerializeField] private float m_colliderRadius;
        [SerializeField] private LayerMask m_layerMask;
    
        public override bool Evaluate(Global.WorldEventType evenType, GameObject source, GameObject target)
        {
            var hit = Physics2D.OverlapCircle(source.transform.position, m_colliderRadius, m_layerMask);
            return hit != null;
        }
    }
}

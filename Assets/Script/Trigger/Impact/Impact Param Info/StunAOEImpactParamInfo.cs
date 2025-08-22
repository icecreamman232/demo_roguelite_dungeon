using UnityEngine;

namespace SGGames.Scripts.Skills
{
    [CreateAssetMenu(fileName = "Stun AOE Param Info", menuName = "SGGames/Impact/Stun AOE")]
    public class StunAOEParamInfo : ImpactParamInfo
    {
        public float StunDuration;
        public float StunRadius;
    }
}

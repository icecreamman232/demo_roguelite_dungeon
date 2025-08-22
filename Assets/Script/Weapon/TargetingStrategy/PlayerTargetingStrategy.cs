using UnityEngine;

namespace SGGames.Scripts.Weapons
{
    public class PlayerTargetingStrategy : ITargetingStrategy
    {
        public Vector3 GetAimDirection(Transform weaponTransform, Transform targetTransform)
        {
            var targetPos = targetTransform.position;
            return (targetPos - weaponTransform.position).normalized;
        }
    }
}

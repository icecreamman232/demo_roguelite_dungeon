using UnityEngine;

namespace SGGames.Scripts.Weapons
{
    public interface ITargetingStrategy
    {
        Vector3 GetAimDirection(Transform weaponTransform, Transform targetTransform);
    }
}

using UnityEngine;

namespace SGGames.Script.Weapons
{
    public interface ITargetingStrategy
    {
        Vector3 GetAimDirection(Transform weaponTransform, Transform targetTransform);
    }
}

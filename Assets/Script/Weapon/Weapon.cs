using UnityEngine;

namespace SGGames.Scripts.Weapons
{
    public class Weapon : MonoBehaviour
    {
        public virtual void InitializeWeapon(IWeaponOwner owner){}
        public virtual void RotateWeapon(Vector3 aimDirection, float aimAngle){}
        public virtual void Attack(){}
        public virtual bool IsAttacking() { return false; }
        protected virtual void UpdateAnimationOnAttack(){}
    }
}


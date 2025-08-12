
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Weapons
{
    public class Weapon : MonoBehaviour
    {
        public virtual void InitializeWeapon(IWeaponOwner owner){}
        public virtual void ChangeState(Global.WeaponState nextState){}
        public virtual void RotateWeapon(Vector3 aimDirection, float aimAngle){}
        public virtual void UpdateAnimationOnAttack(){}

        public virtual void Attack(){}
    }
}


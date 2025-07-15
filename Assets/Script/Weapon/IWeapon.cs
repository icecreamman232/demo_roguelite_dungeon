
using SGGames.Script.Core;

namespace SGGames.Script.Weapons
{
    public interface IWeapon
    {
        void InitializeWeapon(IWeaponOwner owner);
        void ChangeState(Global.WeaponState nextState);
        void UpdateAnimationOnAttack();

        public abstract void Attack();
    }
}


using UnityEngine;

namespace SGGames.Script.Weapons
{
    public interface IWeaponAnimator
    {
        void Initialize(Animator animator);
        void UpdateAnimation();
    }    
}


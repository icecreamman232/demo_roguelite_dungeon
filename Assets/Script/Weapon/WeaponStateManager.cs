using System.Collections.Generic;
using System.Linq;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Weapons
{
    public class WeaponStateManager
    {
        private IWeapon m_weapon;
        private IWeaponState m_currentState;
        private Dictionary<Global.WeaponState, IWeaponState> m_cacheStateDictionary;

        public bool IsReady => m_currentState is WeaponReadyState || m_currentState is WeaponAttackComboState;
        
        public WeaponStateManager(IWeapon weapon, (Global.WeaponState stateType, IWeaponState state)[] states)
        {
            m_weapon = weapon;
            m_cacheStateDictionary = new Dictionary<Global.WeaponState, IWeaponState>();
            m_cacheStateDictionary = states.ToDictionary(x => x.stateType, x => x.state);
            m_currentState = new WeaponReadyState();
            m_cacheStateDictionary.Add(Global.WeaponState.Ready, m_currentState);
        }

        public void SetState(Global.WeaponState newStateType)
        {
            m_currentState?.Exit(m_weapon);
            m_currentState = m_cacheStateDictionary[newStateType];
            m_currentState?.Enter(m_weapon);
        }

        public IWeaponState GetState(Global.WeaponState stateType)
        {
             if(m_cacheStateDictionary.TryGetValue(stateType, out var weaponState))
             {
                 return weaponState;
             }
             Debug.Log($"State {stateType} not found");
             //Fallback to the current state to avoid null;
             return m_currentState;
        }

        public void Update()
        {
            m_currentState?.Update(m_weapon);
        }
    }
}

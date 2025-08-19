using System;
using System.Collections;
using SGGames.Script.Core;
using SGGames.Script.Entity;
using SGGames.Script.Events;
using SGGames.Script.Managers;
using UnityEngine;

namespace SGGames.Script.UI
{
    public class AbilityHUDController : MonoBehaviour
    {
        [SerializeField] private AbilityHUDView m_view;
        [SerializeField] private HudButtonEvent m_hudButtonEvent;
        [SerializeField] private AbilityStateEvent m_abilityStateEvent;
        
        private IEnumerator Start()
        {
            yield return new WaitUntil(HasPlayerCreated);
            Initialize();
        }

        private void OnDestroy()
        {
            m_abilityStateEvent.RemoveListener(OnAbilityStateChanged);
        }

        private bool HasPlayerCreated()
        {
            if (ServiceLocator.HasService<LevelManager>())
            {
                var levelManager = ServiceLocator.GetService<LevelManager>();
                return levelManager.Player != null;
            }
            return false;
        }
        
        private void Initialize()
        {
            m_view.Initialize();
            m_view.ShowDefaultView();
            m_abilityStateEvent.AddListener(OnAbilityStateChanged);
        }

        public void OnClickSpecialButton()
        {
            m_hudButtonEvent.Raise(new HudButtonEventData
            {
                HudButtonType = Global.HudButtonType.SpecialAbilityButton
            });
        }
        
        public void OnClickExecuteButton()
        {
            m_hudButtonEvent.Raise(new HudButtonEventData
            {
                HudButtonType = Global.HudButtonType.ExecuteAbilityButton
            });
        }
        public void OnClickCancelButton()
        {
            m_hudButtonEvent.Raise(new HudButtonEventData
            {
                HudButtonType = Global.HudButtonType.CancelAbilityButton
            });
        }
        
        private void OnAbilityStateChanged(AbilityStateEventData abilityStateEventData)
        {
            if(abilityStateEventData.abilityType != Global.AbilityType.Special) return;
            
            switch (abilityStateEventData.AbilityState)
            {
                case Global.AbilityState.Ready:
                    m_view.ShowDefaultView();
                    break;
                case Global.AbilityState.ShowRange:
                    m_view.ShowExecuteButtons();
                    break;
                case Global.AbilityState.Executing:
                    break;
                case Global.AbilityState.Cooldown:
                    m_view.ShowDefaultView();
                    m_view.ShowCooldown();
                    break;
            }
        }
    }
}

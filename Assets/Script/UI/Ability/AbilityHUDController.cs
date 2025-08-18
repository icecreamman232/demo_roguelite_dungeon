using System.Collections;
using SGGames.Script.Core;
using SGGames.Script.Entity;
using SGGames.Script.Managers;
using UnityEngine;

namespace SGGames.Script.UI
{
    public class AbilityHUDController : MonoBehaviour
    {
        [SerializeField] private AbilityHUDView m_view;
        
        private PlayerController m_playerController;
        
        private IEnumerator Start()
        {
            yield return new WaitUntil(HasPlayerCreated);
            Initialize();
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

        public void OnPressSpecialAbility()
        {
            if (m_playerController.StartSpecialAbility())
            {
                OnSpecialAbilityButtonPressed();
            }
        }

        public void OnPressExecute()
        {
            m_playerController.ExecuteSpecialAbility();
            ShowDefaultView();
        }

        public void OnPressCancel()
        {
            m_playerController.CancelSpecialAbility();
            ShowDefaultView();
        }
        
        private void Initialize()
        {
            m_view.Initialize();
            m_view.ShowDefaultView();
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.OnPressSpecialAbility += OnSpecialAbilityButtonPressed;
            inputManager.OnPressExecute += ShowDefaultView;
            inputManager.OnCancel += ShowDefaultView;
            m_playerController = ServiceLocator.GetService<LevelManager>().Player.GetComponent<PlayerController>();
        }

        private void ShowDefaultView()
        {
            m_view.ShowDefaultView();
        }
        
        private void OnSpecialAbilityButtonPressed()
        {
            m_view.ShowExecuteButtons();
        }
    }
}

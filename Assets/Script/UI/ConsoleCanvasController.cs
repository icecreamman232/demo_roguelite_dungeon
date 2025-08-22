using SGGames.Scripts.Core;
using SGGames.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SGGames.Scripts.UI
{
    public class ConsoleCanvasController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_canvasGroup;
        [SerializeField] private TMP_InputField m_inputField;
        [SerializeField] private ConsoleCheatManager m_cheatManager;

        public static bool IsConsoleOpen;
        
        private void Start()
        {
            m_inputField.onSubmit.AddListener(OnSubmitInput);
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.OnPressOpenConsole += ShowConsole;
            inputManager.OnPressCloseUI += HideConsole;
            
            m_canvasGroup.alpha = 0;
            m_canvasGroup.interactable = false;
            m_canvasGroup.blocksRaycasts = false;
            IsConsoleOpen = false;
        }
        
        private void ShowConsole()
        {
            if (IsConsoleOpen) return;
            
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.DisableGameplayInput();
            m_canvasGroup.alpha = 1;
            m_canvasGroup.interactable = true;
            m_canvasGroup.blocksRaycasts = true;
            m_inputField.text = "";
            EventSystem.current.SetSelectedGameObject(m_inputField.gameObject);
            m_inputField.ActivateInputField();
            IsConsoleOpen = true;
        }
        
        private void HideConsole()
        {
            if (!IsConsoleOpen) return;
            
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.EnableInput();
            m_canvasGroup.alpha = 0;
            m_canvasGroup.interactable = false;
            m_canvasGroup.blocksRaycasts = false;
            IsConsoleOpen = false;
        }

        private void OnSubmitInput(string cheat)
        {
            m_cheatManager.ExecuteCommand(cheat);
            m_inputField.text = "";
            EventSystem.current.SetSelectedGameObject(m_inputField.gameObject);
            m_inputField.ActivateInputField();
        }
    }
}


using SGGames.Script.Core;
using SGGames.Script.Events;
using SGGames.Script.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SGGames.Script.UI
{
    public class PromptButton : MonoBehaviour
    {
        [SerializeField] private InteractEvent m_interactEvent;
        [SerializeField] private CanvasGroup m_canvasGroup;

        private int m_currentInteractLayer;
        private string m_currentInteractTag;
        
        private void Awake()
        {
            m_canvasGroup.alpha = 0;
            m_interactEvent.AddListener(OnReceiveInteractEvent);
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.OnPressInteract += InteractActionOnPerformed;
        }
        

        private void InteractActionOnPerformed()
        {
            if (m_currentInteractLayer == -1) return;
            m_interactEvent.Raise(Global.InteractEventType.Finish,m_currentInteractLayer,m_currentInteractTag);
        }

        private void OnReceiveInteractEvent(Global.InteractEventType eventType, int interactLayer, string interactTag)
        {
            switch (eventType)
            {
                case Global.InteractEventType.Interact:
                    m_canvasGroup.alpha = 1;
                    m_currentInteractLayer = interactLayer;
                    m_currentInteractTag = interactTag;
                    break;
                case Global.InteractEventType.Cancel:
                case Global.InteractEventType.Finish:
                    m_canvasGroup.alpha = 0;
                    break;
            }
        }
    }
}

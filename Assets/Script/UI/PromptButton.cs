using SGGames.Script.Core;
using SGGames.Script.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SGGames.Script.UI
{
    public class PromptButton : MonoBehaviour
    {
        [SerializeField] private InteractEvent m_interactEvent;
        [SerializeField] private CanvasGroup m_canvasGroup;

        private void Awake()
        {
            m_canvasGroup.alpha = 0;
            m_interactEvent.AddListener(OnReceiveInteractEvent);
            var interactAction = InputSystem.actions.FindAction("Interact");
            interactAction.performed += InteractActionOnPerformed;
        }

        private void InteractActionOnPerformed(InputAction.CallbackContext callbackContext)
        {
            m_interactEvent.Raise(Global.InteractEventType.Finish);
        }

        private void OnReceiveInteractEvent(Global.InteractEventType eventType)
        {
            switch (eventType)
            {
                case Global.InteractEventType.InteractWithItem:
                    m_canvasGroup.alpha = 1;
                    break;
                case Global.InteractEventType.Cancel:
                case Global.InteractEventType.Finish:
                    m_canvasGroup.alpha = 0;
                    break;
            }
        }
    }
}

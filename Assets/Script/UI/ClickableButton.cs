using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SGGames.Scripts.UI
{
    public class ClickableButton : ButtonController
    {
        [SerializeField] private UnityEvent m_onClickEvent;

        public override void OnPointerClick(PointerEventData eventData)
        {
            m_onClickEvent?.Invoke();
            base.OnPointerClick(eventData);
        }
    }
}

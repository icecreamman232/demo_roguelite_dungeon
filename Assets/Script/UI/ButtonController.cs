using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SGGames.Script.UI
{
    public class ButtonController : Selectable, IPointerClickHandler
    {
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            
        }
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(null);
            base.OnPointerExit(eventData);
        }
    }
}


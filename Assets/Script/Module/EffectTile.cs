using SGGames.Scripts.Core;
using SGGames.Scripts.Events;
using UnityEngine;

namespace SGGames.Scripts.Modules
{
    public class EffectTile : MonoBehaviour
    {
        [SerializeField] private DisplayEffectTileEvent m_displayEffectTileEvent;
        [SerializeField] private Color m_indicatorColor;
        [SerializeField] private SpriteRenderer m_spriteRenderer;

        private void Awake()
        {
            m_displayEffectTileEvent.AddListener(OnDisplayEffectTile);
        }

        private void OnDestroy()
        {
            m_displayEffectTileEvent.RemoveListener(OnDisplayEffectTile);
        }

        private void ResetDisplay()
        {
            var transparentWhite = new Color(1, 1, 1, 0);
            m_spriteRenderer.color = transparentWhite;
            gameObject.SetActive(false);
        }
        
        private void DisplayIndicator()
        {
            m_spriteRenderer.color = m_indicatorColor;
        }

        private void OnDisplayEffectTile(EffectTileEventData effectTileEventData)
        {
            switch (effectTileEventData.EffectTileType)
            {
                case Global.EffectTileType.None:
                    ResetDisplay();
                    break;
                case Global.EffectTileType.Indicator:
                    DisplayIndicator();
                    break;
                case Global.EffectTileType.Fire:
                    break;
                case Global.EffectTileType.Poison:
                    break;
            }
        }
    }
}

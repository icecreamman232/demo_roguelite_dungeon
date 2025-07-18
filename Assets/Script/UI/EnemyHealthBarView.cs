using System;
using SGGames.Script.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SGGames.Script.UI
{
    public class EnemyHealthBarView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_canvasGroup;
        [SerializeField] private Image m_healthBar;

        private void Start()
        {
            m_canvasGroup.alpha = 0;
        }

        public void UpdateHealthBarVisual(float currentHealth, float maxHealth)
        {
            if (m_canvasGroup.alpha == 0 && currentHealth < maxHealth)
            {
                m_canvasGroup.alpha = 1;
            }
            m_healthBar.fillAmount = MathHelpers.Remap(currentHealth, 0, maxHealth, 0, 1);
            if (m_healthBar.fillAmount <= 0)
            {
                m_healthBar.fillAmount = 0;
                m_canvasGroup.alpha = 0;
            }
        }
    }
}


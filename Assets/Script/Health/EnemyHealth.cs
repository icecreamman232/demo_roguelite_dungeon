using System.Collections;
using SGGames.Script.Data;
using SGGames.Script.Modules;
using SGGames.Script.UI;
using UnityEngine;

namespace SGGames.Script.HealthSystem
{
    public class EnemyHealth : Health
    {
        [Header("Enemy")] 
        [SerializeField] private EnemyData m_enemyData;
        private EnemyHealthBarController m_enemyHealthBar;
        private FillOverlayColorOnSprite m_fillOverlayColorOnSprite;
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        private static float SPRITE_FLICKING_FREQUENCY = 0.1f;
        
        protected override void Start()
        {
            m_maxHealth = m_enemyData.MaxHealth;
            base.Start();
            m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            m_enemyHealthBar = GetComponentInChildren<EnemyHealthBarController>();
            m_fillOverlayColorOnSprite = GetComponentInChildren<FillOverlayColorOnSprite>();
            UpdateHealthBar();
        }

        [ContextMenu("Flick")]
        private void Test()
        {
            StartCoroutine(OnInvincible(5));
        }

        protected override IEnumerator OnInvincible(float duration)
        {
            m_isInvincible = true;
            
            var timeStop = duration + Time.time;
            m_fillOverlayColorOnSprite.FillOverlayColor(m_spriteRenderer,Color.white);
            
            while (Time.time < timeStop)
            {
                m_fillOverlayColorOnSprite.SetIntensity(1);
                yield return new WaitForSeconds(SPRITE_FLICKING_FREQUENCY);
                m_fillOverlayColorOnSprite.SetIntensity(0);
                yield return new WaitForSeconds(SPRITE_FLICKING_FREQUENCY);
            }
            m_fillOverlayColorOnSprite.ResetColor(m_spriteRenderer);
            
            
            m_isInvincible = false;
        }

        protected override void UpdateHealthBar()
        {
            m_enemyHealthBar.UpdateHealthBar(m_currHealth, m_maxHealth);
            base.UpdateHealthBar();
        }
    }
}



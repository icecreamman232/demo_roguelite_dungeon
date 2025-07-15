using System.Collections;
using System.Globalization;
using SGGames.Script.Data;
using SGGames.Script.Modules;
using SGGames.Script.UI;
using UnityEngine;

namespace SGGames.Script.HealthSystem
{
    public class EnemyHealth : Health, IRevivable
    {
        [Header("Enemy")] 
        [SerializeField] private EnemyData m_enemyData;
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        [SerializeField] private FloatingTextEvent m_floatingTextEvent;
        private EnemyHealthBarController m_enemyHealthBar;
        private FillOverlayColorOnSprite m_fillOverlayColorOnSprite;
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
            
            while (Time.time < timeStop)
            {
                m_fillOverlayColorOnSprite.FillOverlayColor(m_spriteRenderer,Color.red,1);
                yield return new WaitForSeconds(SPRITE_FLICKING_FREQUENCY);
                m_fillOverlayColorOnSprite.FillOverlayColor(m_spriteRenderer,Color.red,0);
                yield return new WaitForSeconds(SPRITE_FLICKING_FREQUENCY);
            }
            
            m_fillOverlayColorOnSprite.FillOverlayColor(m_spriteRenderer,Color.white,0);
            
            m_isInvincible = false;
        }

        protected override void Damage(float damage, GameObject source)
        {
            base.Damage(damage, source);
            m_floatingTextEvent.Raise(damage.ToString(), transform.position);
        }

        protected override void UpdateHealthBar()
        {
            m_enemyHealthBar.UpdateHealthBar(m_currHealth, m_maxHealth);
            base.UpdateHealthBar();
        }

        public void OnRevive()
        {
            m_isInvincible = false;
            UpdateHealthBar();
        }
    }
}



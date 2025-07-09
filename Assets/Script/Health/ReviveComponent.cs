using SGGames.Script.Entity;
using SGGames.Script.Modules;
using UnityEngine;


namespace SGGames.Script.HealthSystem
{
    public class ReviveComponent : EntityBehavior
    {
        [SerializeField] private CommandSequencePlayer m_commandSequencePlayer;
        [SerializeField] private int m_maxReviveTime = 1;

        private IRevivable[] m_revivableComponents;
        
        private bool m_isReviving = false;
        private int m_reviveCounter = 0;
        private Health m_health;

        private void Awake()
        {
            m_health = GetComponent<Health>();
            m_revivableComponents = GetComponents<IRevivable>();
            m_commandSequencePlayer.OnCompleteSequence += TriggerReviveActionOnComponents;
        }
        
        private void TriggerReviveActionOnComponents()
        {
            foreach (var revivableComponent in m_revivableComponents)
            {
                if(revivableComponent == null) continue;
                revivableComponent.OnRevive();
            }
            
            m_isReviving = false;
        }

        public bool CanRevive()
        {
            if(m_reviveCounter >= m_maxReviveTime) return false;
            if (m_commandSequencePlayer.IsPlaying) return false;

            return true;
        }

        public void Revive()
        {
            if (!CanRevive()) return;
            
            Debug.Log($"{this.gameObject.name} revived");
            
            m_isReviving = true;
            m_reviveCounter++;
            
            m_health.SetHealth(m_health.MaxHealth);
            
            m_commandSequencePlayer.Play(this.gameObject);
        }
    }
}


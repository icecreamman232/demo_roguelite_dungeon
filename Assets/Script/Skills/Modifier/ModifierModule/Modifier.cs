using System;
using SGGames.Script.Entity;


namespace SGGames.Script.Skills
{
    [Serializable]
    public abstract class Modifier
    {
        protected PlayerController m_playerController;
        protected bool m_shouldBeRemoved;
        
        public bool CanRemove => m_shouldBeRemoved;
        
        public Modifier(PlayerController controller)
        {
            m_playerController  = controller;
        }
        
        public abstract void Apply();
        public virtual void Update(){}
        public abstract void Remove();

    }
}

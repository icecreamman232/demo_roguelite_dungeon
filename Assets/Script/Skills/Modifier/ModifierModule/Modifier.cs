using System;
using SGGames.Script.Entity;


namespace SGGames.Script.Skills
{
    [Serializable]
    public abstract class Modifier
    {
        protected IEntityIdentifier m_entity;
        protected bool m_shouldBeRemoved;
        
        public bool CanRemove => m_shouldBeRemoved;
        
        public Modifier(IEntityIdentifier controller)
        {
            m_entity  = controller;
        }
        
        public abstract void Apply();
        public virtual void Update(){}
        public abstract void Remove();

    }
}

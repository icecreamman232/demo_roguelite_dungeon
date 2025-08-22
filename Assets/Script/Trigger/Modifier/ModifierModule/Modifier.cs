using System;

namespace SGGames.Scripts.Items
{
    [Serializable]
    public abstract class Modifier
    {
        protected IEntityIdentifier m_entity;
        protected bool m_shouldBeRemoved;
        
        public bool CanRemove => m_shouldBeRemoved;
        
        protected Modifier(IEntityIdentifier controller)
        {
            m_entity  = controller;
        }
        
        public abstract void Apply();
        public virtual void Update(){}
        public abstract void Remove();

    }
}

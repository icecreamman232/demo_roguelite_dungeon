using System.Collections.Generic;
using SGGames.Script.Entities;
using SGGames.Script.Skills;


namespace SGGames.Script.Entity
{
    public class PlayerModifierController : EntityBehavior, IModifierController
    {
        private PlayerController m_controller;
        private List<Modifier> m_modifiers;

        private void Start()
        {
            m_modifiers = new List<Modifier>();
        }

        private void Update()
        {
            UpdateModifiers();
        }

        public void Initialize(PlayerController controller)
        {
            m_controller = controller;
        }


        public void AddModifier(ModifierData modifierData)
        {
            var modifier = ModifierFactory.CreateModifier(m_controller, modifierData);
            m_modifiers.Add(modifier);
            modifier.Apply();
        }

        public void UpdateModifiers()
        {
            if (m_modifiers.Count > 0)
            {
                for (int i = m_modifiers.Count - 1; i >= 0; i--)
                {
                    m_modifiers[i].Update();
                    if (m_modifiers[i].CanRemove)
                    {
                        m_modifiers[i].Remove();
                        m_modifiers.RemoveAt(i);
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using SGGames.Script.Core;
using SGGames.Script.Entity;
using SGGames.Script.Managers;
using UnityEngine;

namespace SGGames.Script.Skills
{
    public class ModifierManager : MonoBehaviour, IGameService
    {
        [SerializeField] private ModifierData m_testData;
        private List<Modifier> m_modifierList;
        private PlayerController m_playerController;

        private void Awake()
        {
            ServiceLocator.RegisterService<ModifierManager>(this);
            m_modifierList = new List<Modifier>();
            var lvlManager = ServiceLocator.GetService<LevelManager>();
            m_playerController = lvlManager.Player.GetComponent<PlayerController>();
        }

        private void OnDestroy()
        {
            ServiceLocator.UnregisterService<ModifierManager>();
        }

        private void Update()
        {
            if (m_modifierList.Count > 0)
            {
                for (int i = m_modifierList.Count - 1; i >= 0; i--)
                {
                    m_modifierList[i].Update();
                    if (m_modifierList[i].CanRemove)
                    {
                        m_modifierList[i].Remove();
                        m_modifierList.RemoveAt(i);
                    }
                }
            }
        }

        private void AddModifier(ModifierData data)
        {
            var modifier = ModifierFactory.CreateModifier(m_playerController, data);
            m_modifierList.Add(modifier);
            modifier.Apply();
        }
        
        [ContextMenu("Test")]
        private void Test()
        {
            AddModifier(m_testData);
        }
    }
}

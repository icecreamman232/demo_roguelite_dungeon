using System.Collections.Generic;
using UnityEngine;

namespace SGGames.Scripts.Modules
{
    [CreateAssetMenu(fileName = "Command Sequencer", menuName = "SGGames/Command/Sequencer")]
    public class CommandSequencer : ScriptableObject
    {
        [SerializeField] private ICommand[] m_commands;

        public IEnumerable<ICommand> Commands => m_commands;
    }
}

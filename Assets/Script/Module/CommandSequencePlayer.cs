using System;
using System.Collections;
using UnityEngine;

namespace SGGames.Script.Modules
{
    public class CommandSequencePlayer : MonoBehaviour
    {
        [SerializeField] private CommandSequencer m_commandSequencer;

        public Action OnCompleteSequence;
        
        private Coroutine m_currentSequence;
        private bool m_isPlaying;
        
        public bool IsPlaying => m_isPlaying;
        
        public void Play(GameObject target)
        {
            if (m_isPlaying) return;

            Stop();
            
            m_currentSequence = StartCoroutine(ExecuteSequence(target));
        }

        public void Stop()
        {
            if (m_currentSequence != null)
            {
                StopCoroutine(m_currentSequence);
                m_currentSequence = null;
            }
            m_isPlaying = false;
        }

        private IEnumerator ExecuteSequence(GameObject target)
        {
            m_isPlaying = true;
            
            foreach (var command in m_commandSequencer.Commands)
            {
                if (command == null) continue;
                yield return command.Execute(target);
            }
            
            m_isPlaying = false;
            OnCompleteSequence?.Invoke();
        }
    }
}

using System.Collections;
using UnityEngine;

namespace SGGames.Scripts.Modules
{
    public abstract class ICommand : ScriptableObject
    {
        [SerializeField] protected float m_delay = 0f;
        
        public abstract IEnumerator Execute(GameObject target);
    }
}

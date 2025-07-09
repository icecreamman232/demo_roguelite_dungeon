using System.Collections;
using UnityEngine;

namespace SGGames.Script.Modules
{
    public abstract class ICommand : ScriptableObject
    {
        [SerializeField] protected float m_delay = 0f;
        
        public abstract IEnumerator Execute(GameObject target);
    }
}

using UnityEngine;

namespace SGGames.Scripts.Skills
{
    public abstract class Impact : MonoBehaviour
    {
        public abstract void Initialize(ImpactParamInfo paramInfo);
        public abstract void Execute();
        protected abstract void Finished();
    }
}

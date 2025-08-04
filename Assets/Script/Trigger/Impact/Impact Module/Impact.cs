using UnityEngine;

namespace SGGames.Script.Skills
{
    public abstract class Impact : MonoBehaviour
    {
        public abstract void Initialize(ImpactParamInfo paramInfo);
        public abstract void Execute();
        protected abstract void Finished();
    }
}

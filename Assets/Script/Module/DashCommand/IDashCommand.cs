using UnityEngine;

namespace SGGames.Scripts.Modules
{
    public interface IDashCommand
    {
        void Initialize(GameObject target);
        void Execute();
    }
}

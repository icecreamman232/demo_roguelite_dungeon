using UnityEngine;

namespace SGGames.Script.Modules
{
    public interface IDashCommand
    {
        void Initialize(GameObject target);
        void Execute();
    }
}

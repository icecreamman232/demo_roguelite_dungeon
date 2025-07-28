using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Skills
{
    public class ImpactManager : MonoBehaviour, IGameService
    {
        private void Awake()
        {
            ServiceLocator.RegisterService<ImpactManager>(this);
        }
    }
}

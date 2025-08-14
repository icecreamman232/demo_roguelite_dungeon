using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Script.Modules
{
    public class DisableBrainDeathCommand : IDeathCommand
    {
        private EnemyController m_controller;
    
        public void Initialize(EnemyController controller)
        {
            m_controller = controller;
        }

        public void Execute()
        {
            
            //Debug.Log(("DisableBrainDeathCommand"));
        }
    }

}

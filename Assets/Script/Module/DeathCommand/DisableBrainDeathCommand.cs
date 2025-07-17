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
            if (m_controller.CurrentBrain)
            {
                m_controller.CurrentBrain.ResetBrain();
                m_controller.CurrentBrain.ActivateBrain(false);
            }
            m_controller.CurrentBrain.gameObject.SetActive(false);
            
            //Debug.Log(("DisableBrainDeathCommand"));
        }
    }

}

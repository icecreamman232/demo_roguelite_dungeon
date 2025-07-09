using SGGames.Script.Modules;
using SGGames.Scripts.Entity;

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
            m_controller.CurrentBrain.BrainActive = false;
            m_controller.CurrentBrain.ResetBrain();
            m_controller.CurrentBrain.gameObject.SetActive(false);
        }
    }

}

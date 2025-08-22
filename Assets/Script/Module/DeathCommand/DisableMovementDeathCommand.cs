using SGGames.Scripts.Entities;

namespace SGGames.Script.Modules
{
    public class DisableMovementDeathCommand : IDeathCommand
    {
        private EnemyMovement m_enemyMovement;

        public void Initialize(EnemyController controller)
        {
            m_enemyMovement = controller.gameObject.GetComponent<EnemyMovement>();
        }

        public void Execute()
        {
            m_enemyMovement.StopMoving();
        }
    }
}

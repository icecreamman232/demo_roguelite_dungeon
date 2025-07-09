using SGGames.Scripts.Entity;


namespace SGGames.Script.Modules
{
    public interface IDeathCommand
    {
        void Initialize(EnemyController controller);
        void Execute();
    }
}


namespace SGGames.Script.Modules
{
    /// <summary>
    /// Interface to class that need ability
    /// to change states of a finite state machine
    /// </summary>
    public interface IStateTransitioner<TStateEnum> where TStateEnum : System.Enum
    {
        void ChangeState(TStateEnum newState);
    }
}

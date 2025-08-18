
namespace SGGames.Script.Entity
{
    public interface IPlayerSpecialAbility
    {
        bool StartSpecial();
        void ExecuteSpecial();
        void CancelSpecial();
    }
}

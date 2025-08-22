
namespace SGGames.Scripts.Entities
{
    public interface IPlayerSpecialAbility
    {
        bool StartSpecial();
        void ExecuteSpecial();
        void CancelSpecial();
    }
}

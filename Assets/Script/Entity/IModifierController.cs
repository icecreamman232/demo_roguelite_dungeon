using SGGames.Scripts.Items;

namespace SGGames.Scripts.Entities
{
    public interface IModifierController
    {
        void AddModifier(ModifierData modifierData);
        void UpdateModifiers();
    }
}

using SGGames.Script.Items;

namespace SGGames.Script.Entity
{
    public interface IModifierController
    {
        void AddModifier(ModifierData modifierData);
        void UpdateModifiers();
    }
}

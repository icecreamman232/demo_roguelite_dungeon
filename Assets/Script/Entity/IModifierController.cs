using SGGames.Script.Items;

namespace SGGames.Script.Entities
{
    public interface IModifierController
    {
        void AddModifier(ModifierData modifierData);
        void UpdateModifiers();
    }
}

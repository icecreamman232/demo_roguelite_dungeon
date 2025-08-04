using SGGames.Script.Skills;

namespace SGGames.Script.Entities
{
    public interface IModifierController
    {
        void AddModifier(ModifierData modifierData);
        void UpdateModifiers();
    }
}

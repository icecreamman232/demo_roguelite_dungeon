using System;
using System.Collections.Generic;
using SGGames.Script.Entity;

namespace SGGames.Script.Skills
{
    public static class ModifierFactory
    {
        // Delegate for creating a Modifier
        private delegate Modifier ModifierCreator(PlayerController controller, ModifierData data);

        // Registry mapping ModifierData types to their creation functions
        private static readonly Dictionary<Type, ModifierCreator> ModifierCreators = new Dictionary<Type, ModifierCreator>
        {
            {
                typeof(InvincibilityModifierData),
                (controller, data) => new InvincibilityModifier(controller, ((InvincibilityModifierData)data).Duration)
            }
            // Add more mappings for other Modifier types here
        };

        public static Modifier CreateModifier(PlayerController controller, ModifierData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "ModifierData cannot be null.");
            }

            Type dataType = data.GetType();
            if (ModifierCreators.TryGetValue(dataType, out var creator))
            {
                return creator(controller, data);
            }

            throw new ArgumentException($"No creator registered for ModifierData type: {dataType.Name}");
        }

        // // Optional: Method to register new Modifier types dynamically (e.g., for modding or extensibility)
        // public static void RegisterModifier(Type dataType, ModifierCreator creator)
        // {
        //     ModifierCreators[dataType] = creator;
        // }
    }
}
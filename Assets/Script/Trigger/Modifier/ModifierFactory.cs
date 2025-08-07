using System;
using System.Collections.Generic;
using SGGames.Script.Entity;

namespace SGGames.Script.Items
{
    public static class ModifierFactory
    {
        // Delegate for creating a Modifier
        private delegate Modifier ModifierCreator(PlayerController controller, ModifierData data);
        
        // Registry mapping ModifierData types to their creation functions
        private static readonly Dictionary<Type, ModifierCreator> ModifierCreators = new();
        
        // Static constructor to initialize the dictionary
        static ModifierFactory()
        {
            RegisterModifier<InvincibilityModifierData>(
                (controller, data) => new InvincibilityModifier(controller, data.Duration)
            );
            RegisterModifier<DamageResistanceModifierData>(
                (controller, data) => new DamageResistanceModifier(controller, 
                    ((DamageResistanceModifierData)data).AddingDmgResistance, data.Duration)
            );
            RegisterModifier<DashSpeedEventBasedModifierData>(
                (controller, data) => new DashSpeedEventBasedModifier(controller, 
                    ((DashSpeedEventBasedModifierData)data).WorldEventType, 
                    ((DashSpeedEventBasedModifierData)data).NumberTrigger,
                    ((DashSpeedEventBasedModifierData)data).IsFlatValue,
                    ((DashSpeedEventBasedModifierData)data).BonusSpeed)
            );
            RegisterModifier<PlayerMoveSpeedModifierData>((controller, data) => new PlayerMoveSpeedModifier(controller,
                ((PlayerMoveSpeedModifierData)data).IsFlatValue,
                ((PlayerMoveSpeedModifierData)data).BonusSpeed,
                data.Duration)
            );

            // Add more mappings for other Modifier types here
        }


        private static void RegisterModifier<T>(Func<PlayerController, ModifierData, Modifier> creator)
            where T : ModifierData
        {
            ModifierCreators[typeof(T)] = (controller, data) => creator(controller, (T)data);
        }

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
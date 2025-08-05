using SGGames.Script.EditorExtensions;
using SGGames.Script.Entity;
using SGGames.Script.Items;
using UnityEngine;

[CreateAssetMenu(fileName = "Apply Modifier Action", menuName = "SGGames/Trigger/Action/Apply Modifier")]
public class ApplyModifierAction : TriggerAction
{
    [SerializeField] [ShowProperties] private ModifierData[] m_modifiers;
    
    public override void Execute(GameObject source, GameObject target)
    {
        var identifier = source.GetComponent<IEntityIdentifier>();
        if (identifier.IsPlayer())
        {
            var modifierController = ((PlayerController)identifier).ModifierController;
            foreach (var data in m_modifiers)
            {
                modifierController.AddModifier(data);
            }
        }
    }
}

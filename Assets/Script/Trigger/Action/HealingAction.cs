using SGGames.Script.HealthSystem;
using SGGames.Script.Items;
using UnityEngine;

[CreateAssetMenu(fileName = "Healing Action", menuName = "SGGames/Trigger/Action/Healing")]
public class HealingAction : TriggerAction
{
    [SerializeField] private HealingType m_healingType;
    [SerializeField] private bool m_shouldHealSource;
    [SerializeField] private float m_healingValue;

    private enum HealingType
    {
        HealFlatValue,
        HealPercentageBasedOnMaxHealth,
        HealPercentageBasedOnCurrentHealth,
    }
    
    
    public override void Execute(GameObject source, GameObject target)
    {
        Health sourceHealth = source !=null ? source.GetComponent<Health>() : null;
        Health targetHealth = target !=null ? target.GetComponent<Health>() : null;
        
        switch (m_healingType)
        {
            case HealingType.HealFlatValue:
                HealFlatValue(m_shouldHealSource ? sourceHealth : targetHealth);
                break;
            case HealingType.HealPercentageBasedOnMaxHealth:
                HealPercentageBasedOnMaxHealth(m_shouldHealSource ? sourceHealth : targetHealth);
                break;
            case HealingType.HealPercentageBasedOnCurrentHealth:
                HealPercentageBasedOnCurrentHealth(m_shouldHealSource ? sourceHealth : targetHealth);
                break;
        }
    }

    private void HealFlatValue(Health health)
    {
        if(health == null) return;
        health.Heal(m_healingValue);
    }

    private void HealPercentageBasedOnMaxHealth(Health health)
    {
        if(health == null) return;
        var maxHealth = health.MaxHealth;
        var finalHealingValue = maxHealth * m_healingValue / 100;
        health.Heal(finalHealingValue);
    }
    
    private void HealPercentageBasedOnCurrentHealth(Health health)
    {
        if(health == null) return;
        var maxHealth = health.CurrentHealth;
        var finalHealingValue = maxHealth * m_healingValue / 100;
        health.Heal(finalHealingValue);
    }
}

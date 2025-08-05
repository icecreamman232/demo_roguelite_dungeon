using SGGames.Script.Core;
using SGGames.Script.Items;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash Speed Event Based Modifier Data", menuName = "SGGames/Modifier/Dash Speed Event Based")]
public class DashSpeedEventBasedModifierData : ModifierData
{
    [SerializeField] private Global.WorldEventType m_worldEventType;
    [SerializeField] private int m_numberTrigger;
    [SerializeField] private bool m_isFlatValue;
    [SerializeField] private float m_bonusSpeed;
    
    public Global.WorldEventType WorldEventType => m_worldEventType;
    public int NumberTrigger => m_numberTrigger;
    public bool IsFlatValue => m_isFlatValue;
    public float BonusSpeed => m_bonusSpeed;
}

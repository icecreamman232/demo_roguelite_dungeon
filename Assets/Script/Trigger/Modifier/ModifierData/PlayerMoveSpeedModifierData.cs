using SGGames.Script.Items;
using UnityEngine;

[CreateAssetMenu(fileName = "Player MoveSpeed Modifier Data", menuName = "SGGames/Modifier/Player MoveSpeed")]
public class PlayerMoveSpeedModifierData : ModifierData
{
    [SerializeField] private float m_bonusSpeed;
    [SerializeField] private bool m_isFlatValue;
    
    public float BonusSpeed => m_bonusSpeed;
    public bool IsFlatValue => m_isFlatValue;

}

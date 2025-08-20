using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern Data", menuName = "SGGames/Data/Pattern")]
public class PatternData : ScriptableObject
{
    public int GridWidth;
    public int GridHeight;
    public List<Vector2Int> Pattern;
}

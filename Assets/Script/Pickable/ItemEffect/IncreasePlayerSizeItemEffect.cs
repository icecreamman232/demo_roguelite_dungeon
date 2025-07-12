using UnityEngine;

namespace SGGames.Script.Pickable
{
    [CreateAssetMenu(fileName = "Increase Player Size Item Effect", menuName = "SGGames/Item Effect/Increase Player Size")]
    public class IncreasePlayerSizeItemEffect : ItemEffect
    {
        [SerializeField] private float m_sizeIncrease;
        
        public override void ApplyEffect(GameObject target)
        {
            var model = target.transform.GetChild(0);
            model.localScale += new Vector3(m_sizeIncrease, m_sizeIncrease, 0);
        }

        public override void RemoveEffect(GameObject target)
        {
            var model = target.transform.GetChild(0);
            model.localScale -= new Vector3(m_sizeIncrease, m_sizeIncrease, 0);
        }
    }
}

using UnityEngine;

namespace SGGames.Script.Pickable
{
    [CreateAssetMenu(fileName = "Decrease Player Size Item Effect", menuName = "SGGames/Item Effect/Decrease Player Size")]
    public class DecreasePlayerSizeItemEffect : ItemEffect
    {
        [SerializeField] private float m_sizeDecrease;
        
        public override void ApplyEffect(GameObject target)
        {
            var model = target.transform.GetChild(0);
            model.localScale -= new Vector3(m_sizeDecrease, m_sizeDecrease, 0);
        }

        public override void RemoveEffect(GameObject target)
        {
            var model = target.transform.GetChild(0);
            model.localScale += new Vector3(m_sizeDecrease, m_sizeDecrease, 0);
        }
    }
}
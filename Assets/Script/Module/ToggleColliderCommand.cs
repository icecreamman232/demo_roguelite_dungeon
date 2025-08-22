using System.Collections;
using UnityEngine;

namespace SGGames.Scripts.Modules
{
    [CreateAssetMenu(fileName = "Toggle Collider Command", menuName = "SGGames/Command/Toggle Collider")]
    public class ToggleColliderCommand : ICommand
    {
        [SerializeField] private bool m_toggleColliderValue;
        
        public override IEnumerator Execute(GameObject target)
        {
            if (m_delay > 0f)
            {
                yield return new WaitForSeconds(m_delay);
            }
            
            Collider2D collider = target.GetComponent<Collider2D>();
            collider.enabled = m_toggleColliderValue;
        }
    }

}

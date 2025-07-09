using System.Collections;
using UnityEngine;

namespace SGGames.Script.Modules
{
    [CreateAssetMenu(fileName = "Command Sequencer", menuName = "SGGames/Command/Change Sprite")]
    public class ChangeSpriteCommand : ICommand
    {
        [SerializeField] private Sprite m_spriteChangeTo;
        
        public override IEnumerator Execute(GameObject target)
        {
            if (m_delay > 0f)
            {
                yield return new WaitForSeconds(m_delay);
            }
            
            //Base on hierarchy where the model will always be in the children object
            var spriteRenderer = target.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite = m_spriteChangeTo;
        }
    }

}

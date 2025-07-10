using System.Collections;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Modules
{
    public class AfterImageFX : MonoBehaviour
    {
        [SerializeField] private ObjectPooler m_imagePooler;
        [SerializeField] private float m_delayTimeBetweenImage;

        private bool m_canDropImage = true;
        
        public void DropImageFX(Sprite sprite)
        {
            if (!m_canDropImage) return;

            StartCoroutine(OnCoolDown());
            var cloneGO = m_imagePooler.GetPooledGameObject();
            cloneGO.transform.position = transform.position;
            var playerCloneSprite = cloneGO.GetComponent<PlayerCloneSprite>();
            playerCloneSprite.StartWith(sprite);
        }

        private IEnumerator OnCoolDown()
        {
            m_canDropImage = false;
            yield return new WaitForSeconds(m_delayTimeBetweenImage);
            m_canDropImage = true;
        }
    }
}

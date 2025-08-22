using System.Collections;
using SGGames.Script.Modules;
using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.Modules
{
    /// <summary>
    /// FX controller to create after image effect for dash ability
    /// </summary>
    public class AfterImageFX : MonoBehaviour
    {
        [SerializeField] private ObjectPooler m_imagePooler;
        [SerializeField] private float m_delayTimeBetweenImage;

        private bool m_canDropImage = true;
        
        public void DropImageFX(Sprite sprite, bool isFlipped)
        {
            if (!m_canDropImage) return;

            StartCoroutine(OnCoolDown());
            var cloneGO = m_imagePooler.GetPooledGameObject();
            cloneGO.transform.position = transform.position;
            var playerCloneSprite = cloneGO.GetComponent<PlayerCloneSprite>();
            playerCloneSprite.StartWith(sprite, isFlipped);
        }

        private IEnumerator OnCoolDown()
        {
            m_canDropImage = false;
            yield return new WaitForSeconds(m_delayTimeBetweenImage);
            m_canDropImage = true;
        }
    }
}

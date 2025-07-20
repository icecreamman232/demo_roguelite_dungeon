using System.Collections;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Entity
{
    public class EnemyAnimationController : MonoBehaviour
    {
        private bool m_isFlipped;
        private bool m_isFlipping;
        
        public void FlipModel(bool isFlipped)
        {
            if (m_isFlipping) return;
            if (m_isFlipped == isFlipped) return;
            StartCoroutine(isFlipped ? OnFlippingModel(isFlipped, 0) : OnFlippingModel(isFlipped, 180));
            m_isFlipped = !m_isFlipped;
        }

        private IEnumerator OnFlippingModel(bool isFlipped, float targetAngle)
        {
            m_isFlipping = true;
            var currentAngle = transform.eulerAngles.y;
            var lerpValue = 0f;
            while (lerpValue < 1f)
            {
                lerpValue += Time.deltaTime * Global.S_FLIPPING_MODEL_SPEED;
                currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, lerpValue);
                transform.eulerAngles = new Vector3(0, currentAngle, 0);
                yield return null;
            }
            transform.eulerAngles = new Vector3(0, targetAngle, 0);
            m_isFlipping = false;
        }
    }
}

using UnityEngine;

namespace SGGames.Script.Modules
{
    public class BloodSplashVFX : MonoBehaviour
    {
        [SerializeField] private ParticleSystem m_particleSystem;
        
        public void PlayAtDirection(Vector2 direction)
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            m_particleSystem.Play();
        }
    }
}

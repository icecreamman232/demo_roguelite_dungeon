using UnityEngine;

namespace SGGames.Script.Modules
{
    public class FillOverlayColorOnSprite : MonoBehaviour
    {
        [SerializeField] private Color m_overlayColor;
        [SerializeField] private Material m_ovelayMaterial;
        private static readonly int OverlayColorProperty = Shader.PropertyToID("_OverlayColor");
        private static readonly int IntensityProperty = Shader.PropertyToID("_OverlayIntensity");
        
        private Material m_originalMaterial;

        public void FillOverlayColor(SpriteRenderer targetRender)
        {
            if (m_originalMaterial == null)
            {
                m_originalMaterial = targetRender.material;
            }
            
            targetRender.material = m_ovelayMaterial;
            m_ovelayMaterial.SetColor(OverlayColorProperty, m_overlayColor);
        }

        public void SetIntensity(float intensity)
        {
            m_ovelayMaterial.SetFloat(IntensityProperty, intensity);
        }

        public void ResetColor(SpriteRenderer targetRender)
        {
            targetRender.material = m_originalMaterial;
            targetRender.color = Color.white;
        }
    }
}


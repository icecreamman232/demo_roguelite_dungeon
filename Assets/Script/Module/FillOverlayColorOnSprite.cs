using UnityEngine;

namespace SGGames.Script.Modules
{
    public class FillOverlayColorOnSprite : MonoBehaviour
    {
        private static readonly int OverlayColorProperty = Shader.PropertyToID("_OverlayColor");
        private static readonly int BlendAmountProperty = Shader.PropertyToID("_BlendAmount");

        public void FillOverlayColor(SpriteRenderer targetRender, Color color, float blendAmount)
        {
            targetRender.material.SetColor(OverlayColorProperty,color);
            targetRender.material.SetFloat(BlendAmountProperty,blendAmount);
        }
    }
}


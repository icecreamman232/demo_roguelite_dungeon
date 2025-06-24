using System;
using UnityEngine;
using UnityEngine.UI;

namespace SGGames.Script.Core
{
    /// <summary>
    /// Static helper class which contains method for vary components, scripts and situations.
    /// </summary>
    public static class GameHelper
    {
        #region UI
        public static void SetAlpha(this Image image, float alpha)
        {
            var curColor = image.color;
            curColor.a = alpha;
            image.color = curColor;
        }
        
        public static Color Alpha1(this Color color)
        {
            color.a = 1;
            return color;
        }

        public static Color Alpha0(this Color color)
        {
            color.a = 0;
            return color;
        }
        
        #endregion
        
        #region Randomness
        public static string GetUniqueID()
        {
            return Guid.NewGuid().ToString();
        }
        #endregion
        
        #region Vector

        public static Vector2 With(this Vector2 vector, float? x = null, float? y = null)
        {
            return new Vector2(x ?? vector.x, y ?? vector.y);
        }

        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }

        public static Vector2 Add(this Vector2 vector, float? x = null, float? y = null)
        {
            return new Vector2(vector.x + (x ?? 0), vector.y + (y ?? 0));
        }
        
        public static Vector3 Add(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(vector.x + (x ?? 0), vector.y + (y ?? 0), vector.z + (z ?? 0));
        }
        
        #endregion
    }
}

Shader "SGGames/Outline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range(0, 2)) = 1
        [PerRendererData] _AlphaCutoff ("Alpha Cutoff", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float4 _OutlineColor;
            float _OutlineWidth;
            float _AlphaCutoff;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample the sprite's color
                fixed4 col = tex2D(_MainTex, i.uv);

                // If the pixel is opaque, return it
                if (col.a > _AlphaCutoff)
                {
                    return col;
                }

                // Skip outline processing if width is 0
                if (_OutlineWidth < 0.1)
                {
                    return fixed4(0, 0, 0, 0);
                }

                // Calculate pixel-perfect offset based on outline width
                float width = max(1, _OutlineWidth);
                float2 offset = _MainTex_TexelSize.xy * width;

                // Manually sample 8 neighboring pixels (cardinal and diagonal)
                fixed4 n1 = tex2D(_MainTex, i.uv + float2(-offset.x, 0));     // Left
                fixed4 n2 = tex2D(_MainTex, i.uv + float2(offset.x, 0));      // Right
                fixed4 n3 = tex2D(_MainTex, i.uv + float2(0, -offset.y));     // Down
                fixed4 n4 = tex2D(_MainTex, i.uv + float2(0, offset.y));      // Up
                fixed4 n5 = tex2D(_MainTex, i.uv + float2(-offset.x, -offset.y)); // Bottom-Left
                fixed4 n6 = tex2D(_MainTex, i.uv + float2(-offset.x, offset.y));  // Top-Left
                fixed4 n7 = tex2D(_MainTex, i.uv + float2(offset.x, -offset.y));  // Bottom-Right
                fixed4 n8 = tex2D(_MainTex, i.uv + float2(offset.x, offset.y));   // Top-Right

                // Check if any neighbor is opaque
                float outline = n1.a > _AlphaCutoff || n2.a > _AlphaCutoff || 
                                n3.a > _AlphaCutoff || n4.a > _AlphaCutoff ||
                                n5.a > _AlphaCutoff || n6.a > _AlphaCutoff ||
                                n7.a > _AlphaCutoff || n8.a > _AlphaCutoff;

                // Return outline color if outline is detected, else transparent
                return outline ? _OutlineColor * _OutlineColor.a : fixed4(0, 0, 0, 0);
            }
            ENDCG
        }
    }
    Fallback "Sprites/Default"
}
Shader "Custom/SpriteOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlinePixelWidth ("Outline Pixel Width", Range(0, 10)) = 1
        [Toggle] _ShowOutline ("Show Outline", Float) = 1
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
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
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            fixed4 _OutlineColor;
            float _OutlinePixelWidth;
            float _ShowOutline;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                return OUT;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            fixed4 SampleSpriteTexture(float2 uv)
            {
                float2 pixelUV = floor(uv / _MainTex_TexelSize.xy) * _MainTex_TexelSize.xy;
                return tex2D(_MainTex, pixelUV);
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture(IN.texcoord);
                
                if (_ShowOutline == 1 && c.a == 0)
                {
                    // Calculate UV offset for desired pixel width
                    float pixelOffset = _MainTex_TexelSize.x * _OutlinePixelWidth;

                    // Sample neighboring pixels
                    fixed4 left = SampleSpriteTexture(IN.texcoord + float2(-pixelOffset, 0));
                    fixed4 right = SampleSpriteTexture(IN.texcoord + float2(pixelOffset, 0));
                    fixed4 up = SampleSpriteTexture(IN.texcoord + float2(0, pixelOffset));
                    fixed4 down = SampleSpriteTexture(IN.texcoord + float2(0, -pixelOffset));

                    if (left.a > 0 || right.a > 0 || up.a > 0 || down.a > 0)
                    {
                        return _OutlineColor;
                    }
                }

                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }
    }
}
Shader "Unlit/Grass"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GradientNoise ("GradientNoise", 2D) = "white"
        _Fade ("Fade", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex, _GradientNoise;
            float4 _MainTex_ST;
            float _Fade;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float gradientValue = tex2D(_GradientNoise, i.uv).r;
                _Fade *= .5 * (_SinTime.y + 1);
                float blendFactor = gradientValue * _Fade;
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                fixed4 finalColor1 = lerp(col, fixed4(1, 1, 1, 1), blendFactor);
                return finalColor1;
            }
            ENDCG
        }
    }
}

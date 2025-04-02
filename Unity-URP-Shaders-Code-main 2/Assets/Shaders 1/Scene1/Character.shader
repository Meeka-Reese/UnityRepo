Shader "Unlit/Character"
{
    Properties
    {
        _MainTex ("Texture", 2D) ="transparent" {}
          _HueShift ("HueShift", Range(0.0, 10.0)) = 0.0
    _Saturation ("Saturation", Range(0.0, 5.0)) = 0.0
    _Brightness ("Brightness", Range(-1.0, 1.0)) = 0.0
        _Color ("Color", color) = (0,0,0,0)
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

            sampler2D _MainTex;
            float4 _MainTex_ST, _Color;
            float _Saturation, _Brightness, _HueShift;
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
           float3 HueShifts(float3 color)
{
    float3x3 RGB_YIQ =
        float3x3(0.299, 0.587, 0.114,
                 0.5959, -0.275, -0.3213,
                 0.2115, -0.5227, 0.3112);
    float3x3 YIQ_RGB =
        float3x3(1.0, 0.956, 0.621,
                 1.0, -0.272, -0.647,
                 1.0, -1.106, 1.703);

    float3 YIQ = mul(RGB_YIQ, color);
    float hue = atan2(YIQ.z, YIQ.y) + (_HueShift * 3.14159); // Convert to radians

    float chroma = length(float2(YIQ.y, YIQ.z));
    chroma = lerp(chroma, chroma * _Saturation, _Saturation); // Avoid color loss
    
    float Y = YIQ.x + _Brightness;
    float I = chroma * cos(hue);
    float Q = chroma * sin(hue);

    float3 newRGB = mul(YIQ_RGB, float3(Y, I, Q));
    return newRGB;
}


            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, float2(i.uv.x,i.uv.y));
               
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                col.rgb = HueShifts(col.rgb);
                if (col.a < .1)
                {
                    col = _Color;
                }
                _Color.a = 1.0;

                
                return col;
            }
            ENDCG
        }
    }
}

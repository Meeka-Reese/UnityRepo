Shader "Unlit/Toon"
{
    Properties
    {
       _MainTex ("Texture", 2D) = "white" {}
    _SecondTex ("Texture", 2D) = "white" {}
    _GradientNoise ("Gradient", 2D) = "white" {}
 
    _SecondGrad ("SecondGrad", 2D) = "white" {}
        _Brightness2("Brightness", Range(0,1)) = 0.3
        _Strength("Strength", Range(0,1)) = 0.5
        _Color("Color", COLOR) = (1,1,1,1)
        _Detail("Detail", Range(0,1)) = 0.3
         _Fade ("Fade", Float) = 1
    _Fade2 ("Fade2", Float) = 1
    _SquiggleImage ("SquiggleImage", 2D) = "white" {}
    _TimeScale ("TimeScale", Float) = 1
    _SquiggleAmount ("SquiggleAmount", Float) = 1
    _HueShift ("HueShift", Range(0.0, 10.0)) = 0.0
    _Saturation ("Saturation", Range(0.0, 5.0)) = 0.0
    _Brightness ("Brightness", Range(-1.0, 1.0)) = 0.0
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half3 worldNormal: NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Brightness;
            float _Strength;
            float4 _Color;
            float _Detail;
             sampler2D  _GradientNoise, _SecondTex, _SecondGrad, _SquiggleImage;
            float _Fade, _Fade2, _TimeScale, _SquiggleAmount, _Saturation, _Brightness2, _HueShift;

            float Toon(float3 normal, float3 lightDir, float2 uv) {
   
              float squiggleValue = tex2D(_SquiggleImage, uv).r; // Use the red channel or another channel
                float3 modifiedNormal = normal * squiggleValue;
                 float NdotL = max(0.0, dot(normalize(modifiedNormal), normalize(lightDir + 5)));
                 return floor(NdotL / _Detail);
}


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            
             float3 HueShifts(float3 color)
            {
                float3x3 RGB_YIQ =
                    float3x3 (0.299, 0.587, 0.114,
                              0.5959, -0.275, -0.3213,
                              0.2115, -0.5227, 0.3112);
                float3x3 YIQ_RGB =
                    float3x3 (1, 0.956, 0.619,
                              1, -0.275, -0.3213,
                              1, -1.106, 1.702);

                float3 YIQ = mul(RGB_YIQ, color);
                float hue = atan2(YIQ.z, YIQ.y) + _HueShift;
                float Chroma = length(float2(YIQ.y, YIQ.z) * _Saturation);
                float Y = YIQ.x + _Brightness2;
                float I = Chroma * sin(hue);
                float Q = Chroma * cos(hue);
                
                float3 shiftYIQ = float3(Y,I,Q);
                float3 newRGB = mul(YIQ_RGB, shiftYIQ);
                
                return newRGB;
            }
            

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 ToonCol = _Color;
                ToonCol *= Toon(i.worldNormal, _WorldSpaceLightPos0.xyz, i.uv) * _Strength * _Color + _Brightness;
         
                 // Sample the gradient texture to get the fade value
                float gradientValue = tex2D(_GradientNoise, i.uv).r;
                float gradientValue2 = tex2D(_SecondGrad, i.uv).r;
                // Using the red channel of the gradient
                
                // Sample the main texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col2 = tex2D(_SecondTex, i.uv);
               
                _Fade *= .75 * (_SinTime.y * 1.2 + 1);
                _Fade2 *= .5 *(_SinTime.z  * 1.2+ 2);
                // Calculate the blend factor based on the gradient and fade
                float blendFactor = gradientValue * _Fade;
                float blendFactor2 = gradientValue2 * _Fade2;
                
                // Lerp between the main texture color and white based on the blend factor
                fixed4 finalColor1 = lerp(col, fixed4(1, 1, 1, 1), blendFactor);
                fixed4 finalColor2 = lerp(col2, fixed4(1, 1, 1, 1), blendFactor2);
                fixed4 FinalFinal= (finalColor2 * finalColor1);
                
                FinalFinal.rgb = HueShifts(FinalFinal.rgb);
            

                
              
                // Apply fog
                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                
                return fixed4(FinalFinal * ToonCol); // Map UVs to RGB, with alpha = 1
            }
            ENDCG
        }
    }
}
Shader "Unlit/Lake"
{
    Properties
{
    _MainTex ("Texture", 2D) = "white" {}
    _SecondTex ("Texture", 2D) = "white" {}
    _GradientNoise ("Gradient", 2D) = "white" {}
 
    _SecondGrad ("SecondGrad", 2D) = "black" {}
   
    _Fade ("Fade", Float) = 1
    _Fade2 ("Fade2", Float) = 1
    _SquiggleImage ("SquiggleImage", 2D) = "white" {}
    _TimeScale ("TimeScale", Float) = 1
    _SquiggleAmount ("SquiggleAmount", Float) = 1
    _HueShift ("HueShift", Range(0.0, 10.0)) = 0.0
    _Saturation ("Saturation", Range(0.0, 5.0)) = 0.0
    _Brightness ("Brightness", Range(-1.0, 1.0)) = 0.0
    _RippleTimeScale ("RippleTimeScale", Float) = 500
    _Scale ("Scale", Float) = 1
    _WaveHeight ("WaveHeight", Float) = 1
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

            sampler2D _MainTex, _GradientNoise, _SecondTex, _SecondGrad, _SquiggleImage;
            float _Fade, _Fade2, _TimeScale, _SquiggleAmount, _Saturation, _Brightness, _HueShift, _RippleTimeScale, _Scale, _WaveHeight;
            
            
            
            float4 _MainTex_ST;
            float4 WhiteBright (float2 uv)
            {
                float3 White = (1,1,1);
                return (White, uv.y * uv.x);
                
            }
            float3 Radial(float2 uv)
            {
                
                float time = _RippleTimeScale * _Time.y; // Ripple time scale
                
                float3 SquiggleNoise = tex2D(_SquiggleImage, (uv * 3) + time * .1);
                float2 newUv = (uv * _Scale - _Scale/2) * 50;
                newUv.y = newUv.y - 12;
                float w1 = sin(newUv.x + time + SquiggleNoise.r);
                float w2 = sin(newUv.y + (w1 * .7) + time) * .5;
                float w3 = sin(newUv.x + newUv.y + time );
                float FinalValue = (w1+ w2 + w3);
                float r = sin(sqrt((newUv.x * newUv.x * 1) + newUv.y * newUv.y * .5) - time * 7 + (FinalValue * .4));
                return float3(FinalValue,FinalValue, FinalValue)*.7 + r; // Return modified UVs
            }

           v2f vert (appdata v)
{
    v2f o;
    o.uv = v.uv;

    // Compute radial gradient without sampling textures
    float2 newUv = (o.uv * _Scale - _Scale / 2) * 50;
    newUv.y = newUv.y - 12;
    float w1 = sin(newUv.x + _Time.y * _RippleTimeScale);
    float w2 = sin(newUv.y + (w1 * 0.7) + _Time.y * _RippleTimeScale) * 0.5;
    float w3 = sin(newUv.x + newUv.y + _Time.y * _RippleTimeScale);
    float FinalValue = (w1 + w2 + w3);
    float r = sin(sqrt((newUv.x * newUv.x * 1) + newUv.y * newUv.y * 0.5) - _Time.y * 7 + (FinalValue * 0.4));

    // Radial UV (without texture sampling)
    float radialValue = FinalValue * 0.7 + r;

    // Displace vertex position on Y-axis
    float3 vert = v.vertex;
    vert.z = radialValue * .0005 * _WaveHeight;

    // Pass to clip space
    o.vertex = UnityObjectToClipPos(vert);

    // UV transformation
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    UNITY_TRANSFER_FOG(o, o.vertex);
    return o;
}

            float2 UVDis (float2 uv)
            {
                float time = _Time.y * _TimeScale;
                float w1 = uv.x + time;
                float w2 = uv.y + time;
                
                float3 NewUV = (w1, w2);
                return (NewUV);
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
                float Y = YIQ.x + _Brightness;
                float I = Chroma * sin(hue);
                float Q = Chroma * cos(hue);
                
                float3 shiftYIQ = float3(Y,I,Q);
                float3 newRGB = mul(YIQ_RGB, shiftYIQ);
                
                return newRGB;
            }
            

            fixed4 frag (v2f i) : SV_Target
            {
                float3 radialUV = Radial(i.uv);
                float2 newuv = UVDis(i.uv);
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
                if (radialUV.r > .7)
                {
                    FinalFinal.rgb = (1,1,1);
                }

                
              
                // Apply fog
                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                
                return fixed4(FinalFinal); // Map UVs to RGB, with alpha = 1
            }
            ENDCG
        }
    }
}

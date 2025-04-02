Shader "Unlit/Path"
{
    Properties
{
    _MainTex ("Texture", 2D) = "white" {}
    _SecondTex ("Texture", 2D) = "white" {}
    _GradientNoise ("Gradient", 2D) = "white" {}
    _SecondGrad ("SecondGrad", 2D) = "white" {}
    _Fade ("Fade", Float) = 1
    _Fade2 ("Fade2", Float) = 1
    _SquiggleImage ("SquiggleImage", 2D) = "white" {}
    _TimeScale ("TimeScale", Float) = 1
    _SquiggleAmount ("SquiggleAmount", Float) = 1
    _HueShift ("HueShift", Range(0.0, 10.0)) = 0.0
    _Saturation ("Saturation", Range(0.0, 5.0)) = 0.0
    _Brightness ("Brightness", Range(-1.0, 1.0)) = 0.0
    _Texture ("Texture", 2D) = "black" {}
    _SpotThreshold1 ("SpotThreshold1", float) = .5
    _SpotThreshold2 ("SpotThreshold2", float) = .5
    _Spot1Color("Spot1Color", Color) = (1,1,1,1)
    _Spot2Color("Spot2Color", Color) = (1,1,1,1)
    _Spot3Color("Spot3Color", Color) = (1,1,1,1)
    _Spot4Color("Spot4Color", Color) = (1,1,1,1)
    _Spot5Color("Spot5Color", Color) = (1,1,1,1)
    _Ramp("Ramp", Float) = 1
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

            sampler2D _MainTex, _GradientNoise, _SecondTex, _SecondGrad, _SquiggleImage, _Texture;
            float _Fade, _Fade2, _TimeScale, _SquiggleAmount, _Saturation, _Brightness, _HueShift, _SpotThreshold1, _SpotThreshold2, _Ramp;
            fixed4 _Spot1Color, _Spot2Color, _Spot3Color, _Spot4Color, _Spot5Color;
            
            
            float4 _MainTex_ST;
            float4 WhiteBright (float2 uv)
            {
                float3 White = (1,1,1);
                return (White, uv.y * uv.x);
                
            }

            v2f vert (appdata v)
            {
                
                v2f o;
                o.uv = v.uv;
                float noise = tex2Dlod(_SquiggleImage, float4(o.uv.xy,0,1));
                noise = noise;
                o.uv.x = noise;
                float3 vert = v.vertex;
                vert.z += o.uv.x * .01 * _SquiggleAmount;
               
                
                o.vertex = UnityObjectToClipPos((v.vertex +vert) * .5);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
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
                float SquiggleValue = tex2D(_SquiggleImage, i.uv).r;
                float2 newuv = i.uv * 1.5;
                 newuv.y *= .4;
                float2 newuv2 = i.uv * 2;
                newuv2.y *= .7;
                newuv2.x *= .9;
                newuv2.x += .1;
                 // Sample the gradient texture to get the fade value
                float gradientValue = tex2D(_GradientNoise, i.uv).r;
                float gradientValue2 = tex2D(_SecondGrad, i.uv).r;
                fixed4 Texture = tex2D(_Texture, newuv  * 1+  .01 * SquiggleValue);
                fixed4 Texture2 = tex2D(_Texture, (newuv) * 1.3 + .2);
                fixed4 Texture3 = tex2D(_Texture, newuv  * .3+  .01 * SquiggleValue + .4);
                fixed4 Texture4 = tex2D(_Texture, newuv2  * .1+  .013);
                fixed4 Texture5 = tex2D(_Texture, newuv2 + .12 * .6);
                
                // Using the red channel of the gradient
                
                // Sample the main texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col2 = tex2D(_SecondTex, i.uv);
               
                _Fade *= .75 * (_SinTime.y + 1);
                _Fade2 *= .5 *(_SinTime.z + 2);
                // Calculate the blend factor based on the gradient and fade
                float blendFactor = gradientValue * _Fade;
                float blendFactor2 = gradientValue2 * _Fade2;
                
                // Lerp between the main texture color and white based on the blend factor
                fixed4 finalColor1 = col;
                fixed4 finalColor2 = col2;
                fixed4 FinalFinal= (finalColor2 * finalColor1);
                if (Texture3.r < _SpotThreshold1)
                {    
                    FinalFinal.rgb += _Spot3Color;
                }
                if (Texture3.r < _SpotThreshold1 && Texture3.r > -_SpotThreshold1 + .1)
                {
                    float DarkAmount = -Texture3.r + .5;
                    DarkAmount = lerp(DarkAmount, 1, DarkAmount * 1.5);
                    DarkAmount = lerp(DarkAmount, 1, _Ramp);
                    FinalFinal.rgb *= DarkAmount;
                }
                
                bool splot = false;
                
                if (Texture.r < _SpotThreshold1)
                {    
                    FinalFinal.rgb += _Spot2Color;
                }
                if (Texture.r < _SpotThreshold1 && Texture.r > -_SpotThreshold1 + .1)
                {
                    float DarkAmount = -Texture.r + .5;
                    DarkAmount = lerp(DarkAmount, 1, DarkAmount * 1.5);
                    DarkAmount = lerp(DarkAmount, 1, _Ramp);
                    FinalFinal.rgb *= DarkAmount;
                }

                
                if (Texture4.r < _SpotThreshold1)
                {    
                    FinalFinal.rgb += _Spot4Color;
                }
                if (Texture4.r < _SpotThreshold1 && Texture4.r > -_SpotThreshold1 + .1)
                {
                    float DarkAmount = -Texture4.r + .5;
                    DarkAmount = lerp(DarkAmount, 1, DarkAmount * 1.5);
                    DarkAmount = lerp(DarkAmount, 1, _Ramp);
                    FinalFinal.rgb *= DarkAmount;
                }
                if (Texture5.r < _SpotThreshold1)
                {    
                    FinalFinal.rgb += _Spot5Color;
                }
                if (Texture5.r < _SpotThreshold1 && Texture5.r > -_SpotThreshold1 + .1)
                {
                    float DarkAmount = -Texture5.r + .3;
                    DarkAmount = lerp(DarkAmount, 1, DarkAmount * 1.5);
                    DarkAmount = lerp(DarkAmount, 1, _Ramp);
                    FinalFinal.rgb *= DarkAmount;
                }
                FinalFinal.rgb = HueShifts(FinalFinal.rgb);
                 FinalFinal = lerp(FinalFinal, fixed4(1, 1, 1, 1), blendFactor * .5);
                FinalFinal = lerp(FinalFinal, fixed4(1, 1, 1, 1), blendFactor2 * .5);
                
                
              
               
                
                return (FinalFinal);
            }
            ENDCG
        }
    }
}

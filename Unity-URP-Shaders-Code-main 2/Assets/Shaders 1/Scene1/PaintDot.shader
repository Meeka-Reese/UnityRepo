Shader "Unlit/PaintDot"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("NoiseTex", 2D) = "white" {}
        _VoronoiNoise ("VoronoiNoise", 2D) = "white" {}
        _AniLength ("AniLength", float) = 1
        _NoiseAmount ("NoiseAmount", float) = 1
        _OuterTime ("OuterTime", float) = 0
        _Interval ("Interval", float) = 0
        _Color1 ("Color1", color) = (1,1,1,1)
        _Color2 ("Color2", color) = (1,1,1,1)
        _GradientNoise ("Gradient", 2D) = "white" {}
 
        _SecondGrad ("SecondGrad", 2D) = "white" {}
   
        _Fade ("Fade", Float) = 1
        _Fade2 ("Fade2", Float) = 1
        
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

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
                float3 uv : TEXCOORD0;
            };

            struct v2f
            {
                float3 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex, _NoiseTex, _VoronoiNoise,  _SecondGrad,_GradientNoise;
            float4 _MainTex_ST, _Color1, _Color2;
            float _AniLength, _NoiseAmount, _OuterTime, _Interval, _Fade, _Fade2;
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv.xy, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.uv.z = v.uv.z;
                return o;
            }
           float LoopingTime(float time, float duration)
            {
                // Normalize time to a 0-1 range
                float normalizedTime = frac(time / duration) * .3 / _AniLength;
                float lingerThreshold = 0.9;
                float revTime = .8;
               
                float easedTime;
                if (easedTime <= lingerThreshold)
                {
                easedTime = normalizedTime * normalizedTime * (3.0 - 2.0 * normalizedTime);
                }
                
                
                return clamp((easedTime * duration), 0.0, .9);
            }
            



            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color1 = _Color1;
                fixed4 color2 = _Color2;
                float gradientValue = tex2D(_GradientNoise, i.uv.xy).r;
                float gradientValue2 = tex2D(_SecondGrad, i.uv.xy).r;
                _Fade *= .75 * (_SinTime.y * 1.2 + 1);
                _Fade2 *= .5 *(_SinTime.z  * 1.2+ 2);
                // Calculate the blend factor based on the gradient and fade
                float blendFactor = gradientValue * _Fade;
                float blendFactor2 = gradientValue2 * _Fade2;
                
                // Lerp between the main texture color and white based on the blend factor
                fixed4 finalColor1 = lerp(color1, fixed4(1, 1, 1, 1), blendFactor);
                fixed4 finalColor2 = lerp(color2, fixed4(1, 1, 1, 1), blendFactor2);
                fixed4 FinalFinal= (finalColor2 * finalColor1);
                
                bool FadeRunning = false;
                float FirstTime;
                bool Reset;
                float AddTrans = 0;
                float Noise = tex2D(_NoiseTex, i.uv.xy).r;
                float BlobNoise = tex2D(_VoronoiNoise, i.uv.xy * .5).r;
                BlobNoise = 1 - BlobNoise;
                Noise *= _NoiseAmount * .1;
                float2 newuv = i.uv;
                newuv.y += BlobNoise * .1;
                fixed4 col = tex2D(_MainTex, newuv);
                col.a += Noise;
                _Interval = LoopingTime(_Time.y, 9 * _AniLength);
                if (col.a < .1)
                {
                    col.a = 0;
                }
                if (col.a > 1.05 - clamp((i.uv.z * 1.3), 0, 1) && col.a < 1.10 - clamp((i.uv.z * 1.3), 0, 1))
                {
                    FinalFinal.rgb * .2;
                    AddTrans = .2;
                }
                if (col.a > 1.10 - clamp((i.uv.z * 1.3), 0, 1) && col.a < 1.15 - clamp((i.uv.z * 1.3), 0, 1))
                {
                    FinalFinal.rgb * .7;
                    AddTrans = .1;
                }
                if (col.a > 1.05 - clamp((i.uv.z * 1.3), 0, 1) && !FadeRunning)
                {
                    col.a = 1;
                }
                if (_Interval < .1)
                {
                    FadeRunning = false;
                }
                
                if (_Interval > .85)
                {
                    FadeRunning = true;
                    float TimeReset = _Time.y;
                    
                }
                
                
                float newuvz = 1.5 - i.uv.z * 2;
                
                col.a *= clamp(newuvz, 0, 1);
                col.a = clamp((AddTrans + col.a),0,1);
                FinalFinal.a = col.a;
                
                
                return FinalFinal;
            }
            ENDCG
        }
    }
}
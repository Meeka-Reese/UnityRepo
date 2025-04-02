Shader "Neitri/Procedural Space Skybox"  
{
    Properties 
    {
        _SunSize ("_SunSize", Range(0,1)) = 0.04
         _SecondGrad ("SecondGrad", 2D) = "white" {}
            _GradientNoise ("Gradient", 2D) = "white" {}
        _Fade ("Fade", Float) = 1
        _Fade2 ("Fade2", Float) = 1
        _AddedOrange ("_AddedOrange", Color) = (1,1,1,1)
    
        _TimeScale ("TimeScale", Float) = 1
       

    }

    SubShader 
    {
        Tags 
        {
            "Queue"="Background"
            "RenderType"="Background"
            "PreviewType"="Skybox"
        }
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            float _SunSize;
             sampler2D  _GradientNoise, _SecondGrad;
            float _Fade, _Fade2, _TimeScale;
            float4 _AddedColor;
           

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0; // Add UV coordinates
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0; // Use TEXCOORD0 for UVs
                float3 vertex : TEXCOORD1; // Use TEXCOORD1 for vertex data
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.uv = v.uv;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.pos = UnityObjectToClipPos(v.vertex);
                
                o.vertex = v.vertex;
                 // Transform UVs using _Moon's tiling and offset
                return o;
            }

            // taken from https://www.shadertoy.com/view/4sBXzG
            float hash(float n) { return frac(sin(n) * 1e4); }
            float hash(float2 p) { return frac(1e4 * sin(17.0 * p.x + p.y * 0.1) * (0.1 + abs(sin(p.y * 13.0 + p.x)))); }
            // 1 octave value noise
            float noise(float x) { float i = floor(x); float f = frac(x); float u = f * f * (3.0 - 2.0 * f); return lerp(hash(i), hash(i + 1.0), u); }
            float noise(float2 x) { float2 i = floor(x); float2 f = frac(x);	float a = hash(i); float b = hash(i + float2(1.0, 0.0)); float c = hash(i + float2(0.0, 1.0)); float d = hash(i + float2(1.0, 1.0)); float2 u = f * f * (3.0 - 2.0 * f); return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y; }
            float noise(float3 x) { const float3 step = float3(110, 241, 171); float3 i = floor(x); float3 f = frac(x); float n = dot(i, step); float3 u = f * f * (3.0 - 2.0 * f); return lerp(lerp(lerp(hash(n + dot(step, float3(0, 0, 0))), hash(n + dot(step, float3(1, 0, 0))), u.x), lerp(hash(n + dot(step, float3(0, 1, 0))), hash(n + dot(step, float3(1, 1, 0))), u.x), u.y), lerp(lerp(hash(n + dot(step, float3(0, 0, 1))), hash(n + dot(step, float3(1, 0, 1))), u.x), lerp(hash(n + dot(step, float3(0, 1, 1))), hash(n + dot(step, float3(1, 1, 1))), u.x), u.y), u.z); }

            // Multi-octave noise function
            float squigglyNoise2(float3 p, int octaves, float persistence) {
                float total = 0.0;
                float frequency = 1.0;	
                float amplitude = 1.0;
                float maxValue = 0.0; // Used for normalizing the result to [0, 1]

                for (int i = 0; i < octaves; i++) {
                    total += noise(p * frequency) * amplitude;
                    maxValue += amplitude;
                    amplitude *= persistence;
                    frequency *= 2.0;
                }

                return total / maxValue; // Normalize the result
            }

            float4 frag (v2f i) : SV_Target
            {
                
                 
                float3 ray = normalize(mul((float3x3)unity_ObjectToWorld, i.vertex));
               

                // Sample the _Moon texture
                float2 uv = saturate(i.uv);
                
                

                // Universe colors
             
                 _Fade *= .5 * (_SinTime.y + 1);
                _Fade2 *= .5 *(_SinTime.z + 1);
                   float redUniv = smoothstep(.6, .9, squigglyNoise2(ray + (_Fade * .1), 5, .5));
                float greenUniv = smoothstep(.4, .9, squigglyNoise2(ray * 4 + (_Fade2 * .1), 5, .5));
                float blueUniv = smoothstep(.5, .9, squigglyNoise2(ray * 2 + (_Fade2 *.1), 5, .5));
                float orangeUniv = smoothstep(.5,.9,squigglyNoise2(ray * 4,3,.5));
                float3 Orange = _AddedColor;
                
                greenUniv *= blueUniv;
                redUniv *= _Fade * .8;
                blueUniv *= _Fade2  * 1.2;
                greenUniv *= _Fade2 * 1.2;
 
                // Calculate the blend factor based on the gradient and fade
                
                float a = redUniv + greenUniv + blueUniv + Orange;
             
                float3 Univ = float3(redUniv, greenUniv, blueUniv);
                Univ = Orange + Univ;

                // Starfield
                float starsWeight = smoothstep(0.96, 1, noise(ray * 200));
                float starsWeight2 = smoothstep(.96, 1, noise(ray * 500));
                starsWeight2 += smoothstep(.96, 1, noise(ray * 495));
                starsWeight2 += smoothstep(.96, 1, noise(ray * 493));
                starsWeight2 += smoothstep(.96, 1, noise(ray * 480));
                starsWeight2 *= redUniv * squigglyNoise2(ray, 2, .5) * 2;
                float starsWeight3 = smoothstep(.96, 1, noise(ray * 300));
                starsWeight3 += smoothstep(.96, 1, noise(ray * 320));
                starsWeight3 += smoothstep(.96, 1, noise(ray * 340));
                starsWeight3 += smoothstep(.96, 1, noise(ray * 350));
                starsWeight3 += smoothstep(.96, 1, noise(ray * 370));
                starsWeight3 *= blueUniv * squigglyNoise2(ray, 2, .5) * 2;

                // Planet and corona
                float3 sunDir = _WorldSpaceLightPos0.xyz;
              
              
            
                Univ = saturate(Univ);
                
                
    

                // Remove stars behind the planet
                starsWeight = saturate(starsWeight);
                starsWeight2 = saturate(starsWeight2);
                starsWeight3 = saturate(starsWeight3);

                // Combine all elements
                float3 color = starsWeight + Univ + starsWeight2 + starsWeight3;

                return float4(color, 1);
            }
            ENDCG
        }
    }

    Fallback Off
}
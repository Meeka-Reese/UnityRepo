Shader "Unlit/FogReal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", color) = (0,0,1,.3)
        _Scale ("Scale", float) = 1
        
        Time ("TimeScale", float) = 1
        WaveScale ("WaveScale", float) = .25
        
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 vpos;
            float _Scale;
            float Time;
            float WaveScale;
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                vpos = o.vertex;
                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            float3 Plasma(float2 uv)
            {
                 float time = _Time.y * Time;
                uv = (uv + .1) * _Scale;
                float w1 = sin((uv.x / 2) + (1.4* time));
                float w2 = sin(uv.y + time);
                float w3 = cos(uv.x + uv.y + time);
                float noise = (sin(.2 * uv.x * cos(uv.x) * w2)) + .5;
                float r = sin(sqrt(((uv.x  * uv.x  * WaveScale))  * ((uv.y * uv.y))+(.7 *time)));
                float SubFinalWave = (w1 + w2 + w3 + noise);
                float N2 = sin(SubFinalWave) * noise / cos(3.12 + time);
                float FinalWave = N2 + SubFinalWave;
                return float3 (sin(FinalWave),1,1);
                
                
                // return float3 (w1,w2, 0);
                //cool effect
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
               
                fixed3 plasma = Plasma(i.uv);
                float3 FinalCol = plasma;
                return fixed4(FinalCol, 1);   
            }
            ENDCG
        }
    }
}

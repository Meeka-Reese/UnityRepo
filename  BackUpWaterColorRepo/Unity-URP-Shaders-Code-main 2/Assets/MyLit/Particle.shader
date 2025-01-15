Shader "Unlit/Particle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ErodeTex("Texture",2D) = "white" {}
        _Feather("Feather", float) = 1
        _OuterColor("OuterColor", color) = (0,0,0,0)
        _OuterBoost("OuterBoost", float) = 1
        _BlackOutline("BlackOutline", color) = (0,0,0,0)
        _TimeScale("TimeScale", float) = 1
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
                float4 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _ErodeTex;
            float4 _MainTex_ST;
            float _Feather, _OuterBoost, _TimeScale;
            float4 _OuterColor, _BlackOutline;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv.xy, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.uv.z =1- (v.uv.z % 10);
                return o;
            }
            float3 UVDis (float3 uv)
            {
                float time = _Time.y * _TimeScale;
                float w1 = sin(uv.x) + time;
                float w2 = cos(uv.y) + time;
                float2 NewUV = (w1, w2);
                return (NewUV,uv.z);
                //unused unless making spicy
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 uvdis = UVDis(i.uv);
                
                // sample the texture
               
                fixed4 col = tex2D(_MainTex, i.uv.xy);
                
                float cutoff = i.uv.z;
                float erode = tex2D(_ErodeTex, i.uv.xy).r;
                // erode = step(erode, cutoff);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                fixed3 paintArea = step(erode - _Feather, cutoff);
                paintArea = saturate(paintArea / fwidth((paintArea)));
                fixed3 burntArea = smoothstep(erode - _Feather, erode + _Feather, cutoff);
                fixed3 OuterColor = lerp(col, _OuterColor * _OuterBoost, paintArea);
                fixed3 color = lerp(OuterColor, _BlackOutline, burntArea);
                fixed alpha = saturate(col.a- step(erode + _Feather,cutoff));
                
                
                return fixed4(color, alpha);
            }
            ENDCG
        }
    }
}

Shader "Unlit/DotShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("NoiseTex", 2D) = "white" {}
        _TransparentArea("TransparentArea", 2D) = "white" {}
        _CircleArea("CircleArea", 2D) = "white" {}
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

            sampler2D _MainTex, _NoiseTex, _TransparentArea, _CircleArea;
            float4 _MainTex_ST;

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
                float2 NewUv = i.uv;
                NewUv.x *= 1.4;
                NewUv.y *= 3.5;
                NewUv.y += -.17f;
                NewUv.x += -.2f;

                float2 NewUv2 = i.uv;
                NewUv2.x *= 2.4;
                NewUv2.y *= 1.5;
                NewUv2.y += .65;
                NewUv2.x += .4;
                
                float Circle = tex2D(_CircleArea, NewUv2).r;
                fixed4 col = (1,1,1,1);
                float RandomValue = tex2D(_NoiseTex, i.uv).r;
                float DotGradient = tex2D(_MainTex, i.uv).r;
                DotGradient *= sin(_Time.x);
                DotGradient += .1;
                float TransparentArea = tex2D(_TransparentArea, NewUv).r;
                if (RandomValue * DotGradient > .1)
                {
                    col.rgb = (0,0,0);
                }
                if (TransparentArea > .5)
                {
                    col.a = 0;
                }
                if (Circle > .4)
                {
                    col.a = 0;
                }
                
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}

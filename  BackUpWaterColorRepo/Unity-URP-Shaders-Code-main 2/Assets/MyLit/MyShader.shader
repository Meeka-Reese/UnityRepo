Shader "Unlit/MyShader"
{
    Properties
    {
        _Color("Test Color", color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _AnimateXY("Animate", Vector) = (0,0,0,0)
        [Enum(UnityEngine.Rendering.BlendMode)]
        _SrcFactor("Src factor", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)]
        _DestinationFactor("Dest Factor", Float) = 10
        [Enum(UnityEngine.Rendering.BlendOp)]
        _Operation("Opp", Float) = 0
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        //source shader output
        //destination - stuff behind 
        //source * fsource * destination *
//        Blend SrcAlpha OneMinusSrcAlpha
// transparent
        Blend [_SrcFactor] [_DestinationFactor]
        BlendOp [_Operation]
        //additive
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
                
                float4 vertex : SV_POSITION;
            };
            

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _AnimateXY;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                _AnimateXY.xy *= frac(float2(_Time.yy));
                o.uv += _AnimateXY.xy * _MainTex_ST.xy;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                
                // return fixed4(uvs, 0 ,1);
                // sample the texture
                float2 uvs = i.uv;
                fixed4 textureColor = tex2D(_MainTex, uvs);
                fixed4 col = textureColor;
               
                return col;
                
                
            }
            ENDCG
        }
    }
}

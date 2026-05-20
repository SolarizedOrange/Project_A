Shader "Custom/UVPaint"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _UVIslands ("UVIslands", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

            struct VertexData
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
            };

            struct Interpolator
            {
                float2 uv: TEXCOORD0;
                float4 vertex: SV_POSITION;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MaintTex_TexelSize;
            float _OffsetUV;
            sampler2D _UVIslands;

            Interpolator vert(VertexData v)
            {
                Interpolator i;
                i.vertex = UnityObjectToClipPos(v.vertex);
                i.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return i;
            }

            fixed4 frag(Interpolator i): SV_TARGET
            {
                // float2 offsets[8] = {
                //     float2(-_OffsetUV, 0),
                //     float2(_OffsetUV, 0),
                //     float2(0, _OffsetUV),
                //     float2(0, -_OffsetUV),
                //     float2(-_OffsetUV, _OffsetUV),
                //     float2(_OffsetUV, _OffsetUV),
                //     float2(_OffsetUV, -_OffsetUV),
                //     float2(-_OffsetUV, -_OffsetUV)
                // };

                return tex2D(_MainTex, i.uv);
            }

            ENDCG
        }
    }
}

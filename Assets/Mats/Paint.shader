Shader "Custom/Paint"
{
    Properties
    {
        _PainterColor ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Cull Off ZWrite Off ZTest Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include  "UnityCG.cginc"

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _PainterColor;
            float _PrepareUV;
            float3 _PainterPosition;
            float _Radius;
            float _Hardness;
            float _Strength;

            struct VertexData
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
            };

            struct Interpolator
            {
                float4 vertex: SV_POSITION;
                float2 uv: TEXCOORD0;
                float4 worldPos: TEXCOORD1;
            };

            float mask(float3 position, float3 center, float radius, float hardness)
            {
                float m = distance(center, position);
                return 1 - smoothstep(radius * hardness, radius, m);
            }

            Interpolator vert (VertexData v)
            {
                Interpolator i;
                i.worldPos = mul(unity_ObjectToWorld, v.vertex);
                i.uv = v.uv;
                float4 uv = float4(0.0, 0.0, 0.0, 1.0);
                uv.xy = float2(1, _ProjectionParams.x) * (i.uv * float2(2.0, 2.0) - float2(1.0, 1.0));
                i.vertex = uv;

                return i;
            }

            fixed4 frag(Interpolator i): SV_TARGET
            {
                UNITY_BRANCH
                if (_PrepareUV > 0)
                {
                    return float4(0.0, 0.0, 0.0, 0.0);
                }
                float4 col = tex2D(_MainTex, i.uv);
                float f = mask(i.worldPos, _PainterPosition, _Radius, _Hardness);
                return lerp(col, float4(1.0, 1.0, 1.0, 1.0), f);
                // float edge = f * _Strength;
                // return lerp(col,  _PainterColor, edge):
            }

            ENDCG
        }
    }
}

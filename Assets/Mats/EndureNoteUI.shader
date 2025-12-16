Shader "Unlit/EndureNoteUI"
{
    Properties
    {
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)

        [HideInInspector] _StartTime ("StartTime", Range(0, 1)) = 0.0
        [HideInInspector] _EndTime ("EndTime", Range(0, 1)) = 0.1
        [HideInInspector] _ZWrite ("_ZWrite", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite [_ZWrite]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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

            float _StartTime;
            float _EndTime;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float dist = length((0.5, 0.5) - i.uv);
                float startCircleSize = lerp(0.5, 0.08, _StartTime);
                float endCircleSize = lerp(0.5, 0.08, _EndTime);

                return fixed4(_Color * (startCircleSize > 0.08) ? 1.0 : 0.5) * step(0.0, min(0.0, dist - startCircleSize)) * (1 - step(0.0, min(0.0, dist - endCircleSize)));
            }
            ENDCG
        }
    }
}

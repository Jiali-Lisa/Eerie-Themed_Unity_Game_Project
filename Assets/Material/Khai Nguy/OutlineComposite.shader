Shader "Custom/OutlineComposite"
{
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            // TO BE USED WITH CUSTOM/OUTLINE

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
            float4 _MainTex_TexelSize;
			TEXTURE2D_SAMPLER2D(_OutlineTexture, sampler_OutlineTexture);
            float4 _OutlineTexture_TexelSize;
            float4x4 unity_MatrixMVP;
			half4 _Color;

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (AttributesDefault v)
            {
                v2f o;
                o.vertex = float4(v.vertex.xy, 0.0, 1.0);
                o.uv = TransformTriangleVertexToUV(v.vertex.xy);
            #if UNITY_UV_STARTS_AT_TOP
                o.uv = o.uv * float2(1.0, -1.0) + float2(0.0, 1.0);
            #endif

                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 main = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                float4 outline = SAMPLE_TEXTURE2D(_OutlineTexture, sampler_OutlineTexture, i.uv);

                // apply chosen color
                outline.rgb *= _Color;

                // makes vertex with outline.a value < 1 even further from outline values
                // differentiate outline and non-outline even more
                float4 output = lerp(main, outline, outline.a);
                return output;
            }

            ENDHLSL
        }
    }
}

Shader "Custom/Outline"
{
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            // TO BE USED WITH CUSTOM/OUTLINECOMPOSITE

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

            // main texture and sampler
			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
            float4 _MainTex_TexelSize;

            // camera depth texture and sampler
			TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
            float4x4 unity_MatrixMVP;

            half _MinDepth;
            half _MaxDepth;
            half _Thickness;
            half4 _EdgeColor;

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
                // get texture value (color)
                float4 main = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                // offsets from the current vertex position
                float offset_positive = +ceil(_Thickness * 0.5);
                float offset_negative = -floor(_Thickness * 0.5);

                // left bound
                float left = _MainTex_TexelSize.x * offset_negative;
                // right bound
                float right = _MainTex_TexelSize.x * offset_positive;
                // top bound
                float top = _MainTex_TexelSize.y * offset_negative;
                // bottom bound
                float bottom = _MainTex_TexelSize.y * offset_positive;


                // uv's of the bounds
                float2 uv0 = i.uv + float2(left, top);
                float2 uv1 = i.uv + float2(right, bottom);
                float2 uv2 = i.uv + float2(right, top);
                float2 uv3 = i.uv + float2(left, bottom);

                // get the depth value of the bounds' uv's in range of 0.0 to 1.0
                float d0 = Linear01Depth(SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, uv0));
                float d1 = Linear01Depth(SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, uv1));
                float d2 = Linear01Depth(SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, uv2));
                float d3 = Linear01Depth(SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, uv3));

                // find the difference between the depth value of the bounds' uv's
                float d = length(float2(d1 - d0, d3 - d2));

                // difference recalculate to differentiate lower and higher values more
                d = smoothstep(_MinDepth, _MaxDepth, d);

                // set rgba all to d
                half4 output = d;

                return output;
            }

            ENDHLSL
        }
    }
}

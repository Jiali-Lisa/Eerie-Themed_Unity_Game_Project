Shader "Custom/PostProcessing"
{
    CGINCLUDE
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

        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            return o;
        }
    ENDCG

    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            float _Radius, _Feather, _Frequency;
            fixed4 _Tint;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // move to center
                float2 newUV = i.uv * 2 - 1;

                // distance to center
                float circle = length(newUV);

                // invert of smoothstep value to show soft transition of the oval
                float mask = 1 - smoothstep(_Radius, _Radius + _Feather, circle);
                float invertedMask = 1 - mask;

                // use the color of the the scene for the inner section
                float3 display = col.rgb * mask;

                // pulsating/flashing effect of the vignetter using sin and _Time
                // changing color and opacity/alpha value
                float3 vignette = invertedMask * (fixed4(_Tint.x * (cos(_Time.y * _Frequency) + 1) / 2, 0, 0, 0) + ((cos(_Time.y * _Frequency + UNITY_PI) + 1) / 2) * col);

                return fixed4(display + vignette, 0);
            }
            ENDCG
        }

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            float _Scale, _TimeScale;

            float3 Distort(float2 uv)
            {
                // allow changing intensity / scale but change the shape itself
                uv = uv * _Scale - _Scale / 2;

                // increase the speed of change
                float time = _Time.y * _TimeScale;

                // the waves
                float wave1 = sin(uv.x + time);
                float wave2 = sin(uv.y + time) * 0.5;
                float wave3 = sin(uv.x + uv.y + time);
                float circle = sin(sqrt(uv.x * uv.x + uv.y * uv.y) + time); // circle wave

                // combine all waves
                float finalValue = wave1 + wave2 + wave3 + circle;

                // offset the wave values to increase distortions
                float3 finalWave =  float3(sin(finalValue * UNITY_PI), cos(finalValue * UNITY_PI), sin(finalValue));
                return finalWave;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 distort = Distort(i.uv);

                // apply the distortion then get the texture color value
                fixed4 col = tex2D(_MainTex, i.uv + distort.rg * 0.01);
                return fixed4(col.rgb, 1);
            }
            ENDCG
        }
    }
}

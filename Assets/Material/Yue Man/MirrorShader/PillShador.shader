Shader "Pill/NewUnlitShader"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcFactor("Src Factor", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)] _DstFactor("Dst Factor", Float) = 10
        [Enum(UnityEngine.Rendering.BlendOp)] _Opp("Operation", Float) = 0

        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {}
        _RevealValue("Reveal", float) = 0
        _Feather("Feather", float) = 0

        //add the color to the erosion shader 
        _EroColor("Erode Color", color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend [_SrcFactor] [_DstFactor]
        BlendOp [_Opp]


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _MaskTex;
            float4 _MaskTex_ST; 
            float _RevealValue, _Feather;
            float4 _EroColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.zw = TRANSFORM_TEX(v.uv, _MaskTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv.xy);
                fixed4 mask = tex2D(_MaskTex, i.uv.zw);
                //add the reveal 
                float revealTop = step(mask.r, _RevealValue + _Feather);
                float revealButton = step(mask.r, _RevealValue - _Feather);
                float revealBetween = revealTop - revealButton;
                float3 showCol = lerp(col.rgb, _EroColor, revealBetween);
                //return fixed4(revealBetween.xxx,1);
                //return the color
                return fixed4(showCol.rgb, col.a * revealButton);
            }
            ENDCG
        }
    } 
}

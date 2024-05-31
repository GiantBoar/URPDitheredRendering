Shader "Hidden/DitherEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DitherTex("Dither Texture", 2D) = "white" {}

        _ColourClamp("Colour Clamp", float) = .2
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            sampler2D _DitherTex;
            float4 _DitherTex_TexelSize;

            float _ColourClamp;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float lum = dot(col, float3(0.299f, 0.587f, 0.114f));

                float2 ditherCoords = i.uv * _DitherTex_TexelSize.xy * _MainTex_TexelSize.zw;
                float ditherLum = tex2D(_DitherTex, ditherCoords);

                float ramp = (lum <= clamp(ditherLum, 0.1f, 0.9f)) ? 0.1f : 0.9f;
                float3 output = float3(ramp, ramp, ramp);

                return float4(output, 1.0f);
            }
            ENDCG
        }
    }
}

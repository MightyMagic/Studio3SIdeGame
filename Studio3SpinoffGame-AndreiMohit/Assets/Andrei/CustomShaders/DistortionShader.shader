Shader "Custom/BlackAndWhiteDistortion"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _DistortionTex ("Distortion Texture", 2D) = "white" {}
        _EmissionColor ("Emission Color", Color) = (1, 1, 1, 1)
        _Strength ("Strength", Float) = 0.1
        _Speed ("Speed", Float) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

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

            sampler2D _MainTex;
            sampler2D _DistortionTex;
            float4 _EmissionColor;
            float _Strength;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 mainColor = tex2D(_MainTex, i.uv);
                float2 distortionUV = i.uv + _Strength * sin(_Speed * _Time.y + i.uv);
                float2 distortion = _Strength * tex2D(_DistortionTex, distortionUV).rg;
                float2 finalUV = i.uv + distortion;
                fixed4 emissionColor = _EmissionColor * tex2D(_DistortionTex, finalUV); // Emissive color affecting distorted texture
                float3 bwValue = mainColor.rgb * 0.21 + mainColor.rgb * 0.72 + mainColor.rgb * 0.07; // Convert main color to black and white
                return fixed4(bwValue + emissionColor.rgb, mainColor.a);
            }
            ENDCG
        }
    }
}

Shader "Unlit/Week9Class"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AmbientColor("Ambient Color", Color) = (1, 1, 1, 1)
        _DiffuseColor("Diffuse Color", Color) = (1, 1, 1, 1)
        _SpecularColor("Specular Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 viewDir : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _AmbientColor;
            float4 _DiffuseColor;
            float4 _SpecularColor;

           

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);

                o.viewDir = WorldSpaceViewDir(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                 fixed4 col = tex2D(_MainTex, i.uv);

                float4 ambientLight = _AmbientColor;
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);

                fixed3 normalizedNormal = normalize(i.normal);

                //float NdotL = dot(_WorldSpaceLightPos0, normalizedNormal);
                float NdotL = saturate(dot(_WorldSpaceLightPos0, normalizedNormal));

                NdotL = NdotL > 0 ? 1 : 0;

                float4 diffuseLight = _DiffuseColor * NdotL;

                //Specular
                float3 normalizedViewDir = normalize(i.viewDir);
                float3 halfVector = normalize(normalizedViewDir + _WorldSpaceLightPos0);
                float HdotL = dot(halfVector, normalizedViewDir);
                HdotL = pow(HdotL, 128);

                float4 specularLight = _SpecularColor * HdotL;

                //Final
                //return diffuseLight;
                //return ambientLight + diffuseLight;
                return col * (ambientLight + diffuseLight + specularLight);
                
            }
            ENDCG
        }
    }
}

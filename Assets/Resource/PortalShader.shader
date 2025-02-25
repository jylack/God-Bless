Shader "Custom/PortalShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _Color("Portal Color", Color) = (1,1,1,1)
        _Distortion("Distortion Strength", Range(0,1)) = 0.2
        _Emission("Emission Strength", Range(0,5)) = 2.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _Color;
            float _Distortion;
            float _Emission;

            float random(float2 st)
            {
                return frac(sin(dot(st.xy, float2(12.9898, 78.233))) * 43758.5453123);
            }

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Noise texture animation
                float2 noiseUV = i.uv + float2(sin(_Time.y), cos(_Time.y)) * _Distortion;
                float noise = tex2D(_NoiseTex, noiseUV).r;
                
                // Apply distortion to UV
                float2 distortedUV = i.uv + (noise - 0.5) * _Distortion;
                fixed4 col = tex2D(_MainTex, distortedUV) * _Color;
                
                // Fragmentation effect for shattered dimension
                float shardEffect = step(0.5, random(i.uv * 10.0));
                col.rgb *= shardEffect;
                
                // Emission effect
                col.rgb *= _Emission;
                col.a *= smoothstep(0.3, 0.7, noise) * shardEffect;

                return col;
            }
            ENDCG
        }
    }
}

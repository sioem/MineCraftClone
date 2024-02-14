Shader "Custom/TriplanarOutline"
{
    Properties {
        _TopTex ("Top Tex", 2D) = "white" {}
        _FrontSideTex ("Front Side Tex", 2D) = "white" {}
        _BottomTex ("Bottom Tex", 2D) = "white" {}
    }

    SubShader {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float3 worldNormal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _TopTex;
            sampler2D _FrontSideTex;
            sampler2D _BottomTex;

            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = mul((float3x3)unity_ObjectToWorld, v.normal);
                return o;
            }

            half4 frag (v2f i) : SV_Target {
                float3 worldNormal = normalize(i.worldNormal);
                float3 absWorldNormal = abs(worldNormal);

                float4 top = tex2D(_TopTex, i.worldPos.yz);
                float4 front = tex2D(_FrontSideTex, i.worldPos.xz);
                float4 side = tex2D(_FrontSideTex, i.worldPos.xy);
                float4 bottom = tex2D(_BottomTex, i.worldPos.yz);

                float4 final = lerp(top, bottom, absWorldNormal.y);
                final = lerp(final, side, absWorldNormal.x);
                final = lerp(final, front, absWorldNormal.z);
                final.a = 1;

                return final;
            }
            ENDCG
        }
    }
}

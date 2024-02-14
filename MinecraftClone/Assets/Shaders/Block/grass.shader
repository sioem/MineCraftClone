// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/grass"
{
   Properties
    {
        _TopTex ("Top Tex", 2D) = "white" {}
        _FrontSideTex ("Front Side Tex", 2D) = "white" {}
        _BottomTex ("Bottom Tex", 2D) = "white" {}
        _TopColor ("Top Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        CGPROGRAM
        #pragma surface surf Lambert
        #pragma target 3.0

        sampler2D _TopTex;
        sampler2D _FrontSideTex;
        sampler2D _BottomTex;
        float4 _TopColor;

        struct Input
        {
            float3 worldPos;
            float3 worldNormal;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            // 윗면과 아랫면을 분리하기 위해 조건문을 사용
            if (IN.worldNormal.y >= 0.99) {
                // 윗면에 대한 색상 또는 텍스처를 적용
                float2 top_uv = float2(IN.worldPos.x, IN.worldPos.z);
                float4 top = tex2D(_TopTex, top_uv);
                top.rgb *= _TopColor.rgb; // 윗면에 색상 적용
                o.Albedo = top;
            }
            else {
                // 아랫면에 대한 색상 또는 텍스처를 적용
                float2 bottom_uv = float2(IN.worldPos.x, IN.worldPos.z);
                float4 bottom = tex2D(_BottomTex, bottom_uv);
                o.Albedo = bottom;
            }

            // 그 외의 면에 대한 텍스처를 계산
            if (IN.worldNormal.y < 0.99 && IN.worldNormal.y > -0.99) {
                float2 front_uv = float2(IN.worldPos.x, IN.worldPos.y);
                float4 front = tex2D(_FrontSideTex, front_uv + 0.5);
                
                float2 side_uv = float2(IN.worldPos.z, IN.worldPos.y);
                float4 side = tex2D(_FrontSideTex, side_uv + 0.5);
                
                // 텍스처 블렌딩
                o.Albedo = lerp(o.Albedo, front, abs(IN.worldNormal.z));
                o.Albedo = lerp(o.Albedo, side, abs(IN.worldNormal.x));
            }

            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

Shader "Custom/tree"
{
    Properties
    {
        _TopTex ("Top Tex", 2D) = "white" {}
        _FrontSideTex ("Front Size Tex", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        #pragma surface surf Lambert
        #pragma target 3.0

        sampler2D _TopTex;
        sampler2D _FrontSideTex;    

        struct Input
        {
            float3 worldPos;
            float3 worldNormal;
        };
        void surf (Input IN, inout SurfaceOutput o)
        {
            float2 top_uv = float2(IN.worldPos.x, IN.worldPos.z);
            float4 top = tex2D (_TopTex, top_uv + 0.5);

            float2 front_uv = float2(IN.worldPos.x, IN.worldPos.y);
            float4 front = tex2D(_FrontSideTex, front_uv + 0.5);

            float2 side_uv = float2(IN.worldPos.z, IN.worldPos.y);
            float4 side = tex2D(_FrontSideTex, side_uv + 0.5);

            o.Albedo = top;
            o.Albedo = lerp(o.Albedo, side, abs(IN.worldNormal.x));
            o.Albedo = lerp(o.Albedo, front, abs(IN.worldNormal.z));
            // o.Albedo = lerp(top, front, abs(IN.worldNormal.z));
            // o.Albedo = lerp(o.Albedo, side, abs(IN.worldNormal.x));
            // o.Albedo = lerp(o.Albedo, bottom, abs(IN.worldNormal.));           

            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

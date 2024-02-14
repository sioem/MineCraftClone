Shader "Custom/Outline_2Pass" {
    Properties{
        _TopTex ("Top Tex", 2D) = "white" {}
        _FrontSideTex ("Front Size Tex", 2D) = "white" {}
        _BottomTex ("Bottom Tex", 2D) = "white" {}

        _MainTex("Albedo", 2D) = "white" {}
        _BumpMap("BumpMap", 2D) = "bump" {}
        _OutlineColor("OutlineColor", Color) = (1,1,1,1)
        _Outline("Outline", Range(0.0005, 0.01)) = 0.01
    }

    SubShader{
        Tags { "RenderType" = "Opaque" }
        Cull Off

        // Pass1
        CGPROGRAM
        #pragma surface surf NoLighting vertex:vert noshadow noambient

        struct Input {
            float4 color : Color;
        };

        float4 _OutlineColor;
        float _Outline;

        void vert(inout appdata_full v)
        {
            v.vertex.xyz += v.normal.xyz * _Outline;
        }

        void surf(Input In, inout SurfaceOutput o)
        {
        }

        float4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            return _OutlineColor;
        }
        ENDCG


        // Pass2
        //Cull back
        CGPROGRAM
        #pragma surface surf Toon noambient

        float4 _Color;
        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _TopTex;
        sampler2D _FrontSideTex;    
        sampler2D _BottomTex;

        struct Input {
            float3 worldPos;
            float3 worldNormal;
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float4 color : Color;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
              float2 top_uv = float2(IN.worldPos.x, IN.worldPos.z);
            float4 top = tex2D (_TopTex, top_uv);

            float2 front_uv = float2(IN.worldPos.x, IN.worldPos.y);
            float4 front = tex2D(_FrontSideTex, front_uv);

            float2 side_uv = float2(IN.worldPos.z, IN.worldPos.y);
            float4 side = tex2D(_FrontSideTex, side_uv);

            float2 bottom_uv = float2(IN.worldPos.x, IN.worldPos.z);
            float4 bottom = tex2D(_BottomTex, bottom_uv);

            o.Albedo = top;
            o.Albedo = lerp(o.Albedo, bottom, -IN.worldNormal.y);
            o.Albedo = lerp(o.Albedo, side, abs(IN.worldNormal.x));
            o.Albedo = lerp(o.Albedo, front, abs(IN.worldNormal.z));
            // o.Albedo = lerp(top, front, abs(IN.worldNormal.z));
            // o.Albedo = lerp(o.Albedo, side, abs(IN.worldNormal.x));
            // o.Albedo = lerp(o.Albedo, bottom, abs(IN.worldNormal.));
            

            o.Alpha = 1;

            // float4 c = tex2D(_MainTex, In.uv_MainTex);
            // o.Albedo = c.rgb;
            // o.Normal = UnpackNormal(tex2D(_BumpMap, In.uv_BumpMap));
            // o.Alpha = c.a;
        }

        float4 LightingToon(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            float ndotl = dot(s.Normal, lightDir) * 0.5 + 0.5;

            if (ndotl > 0.7) {
                ndotl = 1;
            }
            else if (ndotl > 0.4) {
                ndotl = 0.3;
                
            }
            else {
                ndotl = 0;
            }
            float4 final;

            final.rgb = s.Albedo * ndotl * _LightColor0.rgb;
            final.a = s.Alpha;

            return final;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
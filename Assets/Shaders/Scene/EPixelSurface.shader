// Copyright 2020, Keegan Beaulieu

Shader "Custom/EPixelSurface"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "grey" {}
        _Tex2 ("Albedo (RGB)", 2D) = "grey" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf CelShade fullforwardshadows vertex:vert addshadow

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Tex2;

        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        struct Input
        {
            float2 uv_MainTex;
        };

        void vert(inout appdata_full v, out Input o)
        {
            o.uv_MainTex = v.texcoord.xy;
        }

        fixed4 surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;

            return c;
        }

        half4 LightingCelShade(SurfaceOutput s, float3 lightDir, half3 viewDir,
                               float shadowAttenuation)
        {
            float dist = length(lightDir);
            float toLight = dot(s.Normal, normalize(lightDir));

            float lightIntensity = 0;
            lightIntensity += toLight > 0.4;
            lightIntensity *= 0.08;

            lightIntensity *= shadowAttenuation;

            float blueShift = step(lightIntensity, 0.001)*0.035;

            return half4(lightIntensity, lightIntensity,
                         lightIntensity + blueShift, 1.0);
        }

        ENDCG
    }
    FallBack "Diffuse"
}

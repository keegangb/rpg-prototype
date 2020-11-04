Shader "Custom/EPixelGrass"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _ShadowMultiplier ("Shadow Multiplier", Range(0, 1)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull off
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf CelShade fullforwardshadows vertex:vert addshadow

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        float _ShadowMultiplier;

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
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
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
            lightIntensity += toLight > 0.6;
            lightIntensity += toLight > 0.0;
            lightIntensity *= 0.08;

            lightIntensity *= shadowAttenuation/(1.0/_ShadowMultiplier) + (1.0 - _ShadowMultiplier);

            float blueShift = (lightIntensity <= 0.001)*0.065;

            return half4(lightIntensity*1.6, lightIntensity,
                         lightIntensity + blueShift, 1.0);
        }

        ENDCG
    }
    FallBack "Diffuse"
}

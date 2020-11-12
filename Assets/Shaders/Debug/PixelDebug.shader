// Copyright 2020, Keegan Beaulieu

Shader "Custom/PixelDebug"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGBA)", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "ForceNoShadowCasting"="True" 
        }

        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        CGPROGRAM
        #pragma surface surf Shade vertex:vert alpha:fade

        #pragma target 3.0

        sampler2D _MainTex;

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

            float4 world = mul(unity_ObjectToWorld, v.vertex);
            world.z += world.y;
            v.vertex = mul(unity_WorldToObject, world);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

        half4 LightingShade(SurfaceOutput s, float3 lightDir, half3 viewDir,
                            float shadowAttenuation)
        {
            return half4(0, 0, 0, s.Alpha);
        }

        ENDCG
    }
    FallBack "Diffuse"
}
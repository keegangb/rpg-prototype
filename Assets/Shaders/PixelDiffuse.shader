Shader "Custom/PixelDiffuse"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _UnitScale ("Unit Scale", float) = 100
        _Position ("Position", Vector) = (0, 0, 0, 0)
        _CameraPosition ("Camera Position", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert addshadow

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        float4 _Position;
        float4 _CameraPosition;
        float _UnitScale;

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

            float3 delta = _Position.xyz - _CameraPosition.xyz; 
            world.xyz -= delta;

            float3 snapped;
            snapped.x = floor(delta.x*_UnitScale + 0.5)/_UnitScale;
            snapped.y = floor(delta.y*_UnitScale + 0.5)/_UnitScale;
            snapped.z = floor(delta.z*_UnitScale + 0.5)/_UnitScale;

            world.xyz += snapped;

            world.z += world.y;

            v.vertex = mul(unity_WorldToObject, world);
        }

        fixed4 surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;

            return c;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

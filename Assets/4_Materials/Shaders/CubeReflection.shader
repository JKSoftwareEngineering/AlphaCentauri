Shader "Custom/CubeReflection"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_CubeMap("Cube", CUBE) = ""{}
    }
    SubShader
    {
        CGPROGRAM
        #pragma surface surf Lambert

        fixed4 _Color;
        samplerCUBE _CubeMap;

        struct Input
        {
            float3 worldRefl;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = _Color.rgb;
			o.Emission = texCUBE(_CubeMap, IN.worldRefl).rgb;

        }
        ENDCG
    }
    FallBack "Diffuse"
}

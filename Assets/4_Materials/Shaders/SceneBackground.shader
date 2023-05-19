Shader "Custom/SceneBackground"
{
	Properties
	{
		_CubeMap("Cube", CUBE) = ""{}
	}
		SubShader
	{
		CGPROGRAM
		#pragma surface surf Lambert

		samplerCUBE _CubeMap;

		struct Input
		{
			float3 worldRefl;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			o.Emission = texCUBE(_CubeMap, IN.worldRefl).rgb;

		}
		ENDCG
	}
		FallBack "Diffuse"
}

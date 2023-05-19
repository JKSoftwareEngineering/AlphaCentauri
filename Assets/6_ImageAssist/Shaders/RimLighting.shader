Shader "Custom/RimLighting"
{
	// connection to the ui
	Properties
	{
		_Color("Color", Color) = (.5,.5,.5,0)
		_Power("Rim power", Range(0,1)) = .4
	}
	SubShader
	{
		CGPROGRAM
		// basic lighting running in geometry buffer
		#pragma surface surf Lambert
		float4 _Color;
		float _Power;
        struct Input
        {
            float3 viewDir;
			//float3 worldPos;
        };
		// using basic surface output
        void surf (Input IN, inout SurfaceOutput o)
        {
			// in words
			// 1 - gadiant of the dot product of the view point and the normal on the object
			half rim = 1-saturate(dot(normalize(IN.viewDir), o.Normal));
			//scale the rim and make it an emmision
			o.Emission = _Color.rgb * pow(rim,_Power);
        }
        ENDCG
    }
    FallBack "Diffuse"
}

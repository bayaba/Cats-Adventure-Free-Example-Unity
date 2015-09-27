// MatCap Shader, (c) 2013,2014 Jean Moreno

Shader "MatCap/Vertex/Textured Lit"
{
	Properties
	{
		_Color ("Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MatCap ("MatCap (RGB)", 2D) = "white" {}
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		
		#pragma surface surf Lambert vertex:vert 
		
		sampler2D _MainTex;
		sampler2D _MatCap;
		float4 _Color;
		
		struct Input
		{
			half2 uv_MainTex : TEXCOORD0;
			float2 matcapUV;
		};
		
		void vert (inout appdata_full v, out Input o)
		{
			#if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
			UNITY_INITIALIZE_OUTPUT(Input,o);
			#endif
			
			o.matcapUV = float2(dot(UNITY_MATRIX_IT_MV[0].xyz,v.normal),dot(UNITY_MATRIX_IT_MV[1].xyz,v.normal)) * 0.5 + 0.5;
		}
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			half4 mc = tex2D(_MatCap, IN.matcapUV);
			o.Albedo = c.rgb * mc.rgb * _Color.rgb * 2.0;
		}
		ENDCG
	}
	
	Fallback "VertexLit"
}

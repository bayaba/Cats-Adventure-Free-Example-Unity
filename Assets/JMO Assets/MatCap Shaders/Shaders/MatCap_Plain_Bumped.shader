// MatCap Shader, (c) 2013,2014 Jean Moreno

Shader "MatCap/Bumped/Plain"
{
	Properties
	{
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_MatCap ("MatCap (RGB)", 2D) = "white" {}
	}
	
	Subshader
	{
		Tags { "RenderType"="Opaque" }
		
		Pass
		{
			Tags { "LightMode" = "Always" }
			
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"
				
				struct v2f
				{
					float4 pos	: SV_POSITION;
					float2 uv_bump : TEXCOORD0;
					
					float3 c0 : TEXCOORD1;
					float3 c1 : TEXCOORD2;
				};
				
				uniform float4 _BumpMap_ST;
				
				v2f vert (appdata_tan v)
				{
					v2f o;
					o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
					o.uv_bump = TRANSFORM_TEX(v.texcoord,_BumpMap);
					
					TANGENT_SPACE_ROTATION;
					o.c0 = mul(rotation, UNITY_MATRIX_IT_MV[0].xyz);
					o.c1 = mul(rotation, UNITY_MATRIX_IT_MV[1].xyz);
					
					return o;
				}
				
				uniform float4 _Color;
				uniform sampler2D _MatCap;
				uniform sampler2D _BumpMap;
				
				float4 frag (v2f i) : COLOR
				{
					fixed3 normals = UnpackNormal(tex2D(_BumpMap, i.uv_bump));
					half2 capCoord = half2(dot(i.c0, normals), dot(i.c1, normals));
					float4 mc = tex2D(_MatCap, capCoord*0.5+0.5);
					
					return _Color * mc * 2.0;
				}
			ENDCG
		}
	}
	
	Fallback "VertexLit"
}
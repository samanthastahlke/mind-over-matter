// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/DiffMaskShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_DiffTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		return o;
	}

	sampler2D _MainTex;
	sampler2D _DiffTex;

	fixed4 frag(v2f i) : SV_Target
	{
		float2 adjustedUV = i.uv;
		adjustedUV.y = 1.0f - adjustedUV.y;

		fixed4 col = tex2D(_MainTex, i.uv) - tex2D(_DiffTex, i.uv);
		return col;
	}
		ENDCG
	}
	}
}

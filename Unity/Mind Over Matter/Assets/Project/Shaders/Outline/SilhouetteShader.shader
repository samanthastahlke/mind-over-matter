// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/SilhouetteShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_GlowTex ("Texture", 2D) = "white" {}
		_MaskTex ("Texture", 2D) = "white" {}
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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _GlowTex;
			sampler2D _MaskTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				
				float2 adjustedUV = i.uv;

				fixed4 glowCol = tex2D(_GlowTex, adjustedUV);

				fixed4 wht;
				wht.rgba = 1.0f;

				fixed4 maskCol = tex2D(_MaskTex, adjustedUV).a * wht;
				fixed4 silCol = 1.0f - maskCol;

				col += glowCol * silCol;
				return col;
			}
			ENDCG
		}
	}
}

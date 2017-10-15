Shader "Unlit/SolidColourShader"
{
	Properties
	{
		_EmissionColor("Color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		ZWrite On
		Cull Back
		Blend Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			fixed4 _EmissionColor;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col;
				col.r = _EmissionColor.r;
				col.g = _EmissionColor.g;
				col.b = _EmissionColor.b;
				col.a = 1.0f;

				return col;
			}
			ENDCG
		}
	}
}

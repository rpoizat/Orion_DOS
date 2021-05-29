Shader "Unlit/contour"
{
	SubShader
	{
		Tags { "RenderType" = "Opaque" }

		Pass
		{
			Fog{ Mode off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform StructuredBuffer<float3> buffer;
			uniform StructuredBuffer<int> index;
			uniform StructuredBuffer<float> windResistance;
			uniform float3 wind;

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			v2f vert(uint id : SV_VertexID)
			{
				uint nbBrin = index[id] / uint(8);
				uint palier = index[id] % uint(8);
				float3 w = wind * windResistance[nbBrin];

				if (palier < 2)
				{
					w = w * 0.0f;
				}
				else
				{
					if (palier < 4)
					{
						w = w * 0.05f;
					}
					else
					{
						if (palier < 6)
						{
							w = w * 0.2f;
						}
						else
						{
							if (palier < 7)
							{
								w = w * 0.4f;
							}
							else
							{
								w = w * 0.70f;
							}
						}
					}
				}

				float4 pos = float4(buffer[index[id]] + w, 1);
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(pos);

				return OUT;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return float4(0.0f, 0.1f, 0.0f, 1.0f);
			}

			ENDCG
		}
	}
}
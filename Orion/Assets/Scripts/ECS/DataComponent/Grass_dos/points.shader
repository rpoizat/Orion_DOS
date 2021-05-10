Shader "Unlit/points"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
			Fog{ Mode off }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			uniform StructuredBuffer<float3> buffer;

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert (uint id : SV_VertexID)
            {
				float4 pos = float4(buffer[id], 1);
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(pos);

                return OUT;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				return float4(0, 1, 0, 1);
            }

            ENDCG
        }
    }
}

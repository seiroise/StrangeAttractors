Shader "Custom/StrangeAttractor/SimpleInstancing"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
		Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
			#pragma target 5.0

            #include "UnityCG.cginc"
			#include "StrangeAttractor.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float4 color : TEXTCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			float4x4 modelMatrix;
#if SHADER_TARGET >= 45
			StructuredBuffer<Params> buf;
#endif

            v2f vert (appdata v, uint id : SV_InstanceID)
            {
				Params p = buf[id];

				float3 localPosition = v.vertex.xyz * p.size.x + p.position;
				float3 worldPosition = mul(modelMatrix, float4(localPosition, 1.0));

                v2f o;
                o.vertex = mul(UNITY_MATRIX_VP, float4(worldPosition, 1.0));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = p.color;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb *= i.color.rgb;

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}

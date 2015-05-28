

Shader "Custom/Blur GUI Texture" {
	Properties{
		_BlurBuffer("Base (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader{
		Pass{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"

			struct vertexInput {
				float4 vertex : POSITION;
				float4 texcoord0 : TEXCOORD0;
			};

			struct fragmentInput {
				float4 position : SV_POSITION;
				float4 texcoord0 : TEXCOORD0;
			};

			uniform sampler2D _BlurBuffer;
			uniform float4 _Color;

			fragmentInput vert(vertexInput i) {
				fragmentInput o;
				o.position = mul(UNITY_MATRIX_MVP, i.vertex);
				o.texcoord0.x = o.position.x * _ScreenParams.z; // 1.0f + (1.0f / Screen.width)
				o.texcoord0.y = o.position.y * _ScreenParams.w; // 1.0f + (1.0f / Screen.height)

				o.texcoord0.x *= 0.5;
				o.texcoord0.x += 0.5;

				o.texcoord0.y *= 0.5;
				o.texcoord0.y += 0.5;

				return o;
			}

			float4 frag(fragmentInput i) : SV_Target{
				return tex2D(_BlurBuffer, i.texcoord0.xy) * _Color;
			}

		ENDCG
		}
	}
}
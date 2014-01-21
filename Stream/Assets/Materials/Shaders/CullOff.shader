Shader "Custom/CullOff" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200 Cull Off
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float4 _Color;
			
			struct appdata {
				float4 vertex : POSITION;
			};
			
			struct vsout {
				float4 vertex : POSITION;
			};
			
			vsout vert(appdata i) {
				vsout o;
				o.vertex = mul(UNITY_MATRIX_MVP, i.vertex);
				return o;
			}
			float4 frag(vsout i) : COLOR {
				return _Color;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}

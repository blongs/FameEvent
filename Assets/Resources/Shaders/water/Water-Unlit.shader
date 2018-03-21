// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hydra/Water Unlit" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_WiggleTex ("Base (RGB)", 2D) = "white" {}
	_WiggleStrength ("Wiggle Strength", Range (0.01, 0.1)) = 0.01
	_LightMap ("LightMap", 2D) = "white" {}
	_Brightness("Brightness", float) = 1
}
SubShader {
	Tags { "Queue"="Transparent-100" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	LOD 200

	CGINCLUDE
	#include "UnityCG.cginc"

	sampler2D _MainTex;
	sampler2D _WiggleTex;
	sampler2D _LightMap;
	fixed4 _Color;
	half _WiggleStrength;
	half _Brightness;
	
	struct Vertex
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
		float2 uv2 : TEXCOORD1;
	};
	
	struct v2f
	{
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
		half2 uv_wiggle : TEXCOORD1;
		half2 uv_lm : TEXCOORD2;
	};
	
	v2f vert (Vertex v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.uv);
		o.uv_wiggle = MultiplyUV(UNITY_MATRIX_TEXTURE1, v.uv);
		o.uv_wiggle.x -= _SinTime;
		o.uv_wiggle.y += _CosTime;
		o.uv_lm = v.uv2;
		return o;
	}
	
	fixed4 frag (v2f i) : COLOR
	{
		half4 wiggle = tex2D(_WiggleTex, i.uv_wiggle);
		
		half2 tc = i.uv;
		tc.x -= wiggle.r * _WiggleStrength;
		tc.y += wiggle.b * _WiggleStrength * 1.5f;
		
		half4 color = tex2D(_MainTex, tc) * _Color;
		color.rgb *= tex2D(_LightMap, i.uv_lm).rgb;
		color.rgb *= _Brightness;
		return color;
	}
	ENDCG
	
	Pass
	{
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB
		
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest	
		ENDCG
	}
}

Fallback "VertexLit"
}

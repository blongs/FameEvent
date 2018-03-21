Shader "Hydra/Diffuse-Glow" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_NormTex("Bump (RGB)", 2D)="bump" {}
	_GlowTex("Glow (RGB)", 2D)="black" {}
	_GlowColor("Glow Color", Color) = (0.93,0.74,0.1,1)
	_GlowFactor("Glow Factor", float) = 20
	_GlowPower("Glow Power", float) = 3
	_Rotation("Rotation", float) = 0.05
	_FlowX("Flow X", float) = -0.04
	_FlowY("Flow Y", float) = 0.03
	_Scale("Scale", float) = 1
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 200

CGPROGRAM
	#pragma surface surf Lambert vertex:vert
	
	sampler2D _MainTex;
	sampler2D _NormTex;
	sampler2D _GlowTex;
	fixed4 _Color;
	fixed4 _GlowColor;
	float _GlowFactor;
	float _GlowPower;
	float _Rotation;
	float _FlowX;
	float _FlowY;
	float _Scale;
	
	struct Input
	{
		float2 uv_MainTex;
	};

	void vert(inout appdata_full v, out Input o)
	{
		UNITY_INITIALIZE_OUTPUT(Input,o);
	}

	void surf (Input IN, inout SurfaceOutput o)
	{
		half2 uv = IN.uv_MainTex;
		
		half angle = _Time.y * _Rotation;
		
		half sinA = sin(angle);
		half cosA = cos(angle);
		
		half2 RotCenter = float2(0.5,0.5);
		half2 uv2 = uv - RotCenter;
		uv2.x = uv.x * cosA + uv.y * sinA;
		uv2.y = uv.x * sinA - uv.y * cosA;
		uv2 += RotCenter;
		
		uv2 += _Time.y * float2(_FlowX, _FlowY);
		uv2 *= _Scale;
		
		half2 scale = tex2D(_NormTex, uv2);
		
		half3 glow = tex2D(_GlowTex, IN.uv_MainTex) * _GlowColor.rgb;
		glow *= pow(scale.x, _GlowPower) * _GlowFactor;
		
		o.Emission = glow;
		
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	
ENDCG
}

Fallback "VertexLit"
}

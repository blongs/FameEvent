// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Chapter5/SimpleShader" 
{
	SubShader 
	{
		Pass
		{
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

		   //使用一个结构体来定义定点着色器得输入
			struct a2v
			{
				//POSITION语义告诉unity，用模型空间的定点左边填充vertex变量
				float4 vertex :POSITION;
				//NORMLA 语义告诉unity，用模型空间的发现方向填充normal变量
				float3 normal :NORMAL;
				//TEXCOORD0 语义告诉unity，用模型的第一套文理坐标填充texcoord变量
				float4 texcoord : TEXCOORD0;
			};
		
			float4 vert(a2v v):SV_POSITION
			{
				return UnityObjectToClipPos(v.vertex);
			}

			fixed4 frag() : SV_Target
			{
				return fixed4(1.0,1.0,1.0,1.0);
			}
			ENDCG
			/*
			float4 vert(float4 v :POSITION) :SV_POSITION
			{
				return UnityObjectToClipPos(v.vertex);
			}

			fixed4 frag():SV_Target
			{
				return fixed4(1.0,1.0,1.0,1.0);
			}
			ENDCG
			*/
		}
	}
	FallBack "Diffuse"
}

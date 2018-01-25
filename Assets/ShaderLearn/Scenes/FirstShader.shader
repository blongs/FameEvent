Shader "Unlit/FirstShader"
{
	Properties
	{
		// Name ("display name ",PropertyType) = DefaultValue
		_Int("MyInt",Int) = 2
		_Float("MyFloat",Float) = 3.0
		_Range("MyRange",Range(0,2.5)) =3
		_Color("MyColor",Color) = (1,1,1,1)
		_Vector("MyVector",Vector)=(2,2,2,2)
		_2D("My2D",2D) = ""{}
		_Cube("MyCube",Cube) = "white"{}
		_3D("My3D",3D) = "black"{}

	}

	/*
		SubShader
		{
			//可选的  标签
			[Tags]
			//Key Value 的形式保存
			Tags{"TagName 1" = "Value1"  "TagName 2" = "Value2"}

			//渲染队列
			Tags{"Queue" = "Transparent"}

			//对着色器分类
			Tags{"RenderType" = "Opaque"}
			......


			//可选的   状态
			[RenderSetup]
			//设置剔除模式：剔除背面，正面，关闭剔除
			Cull Back|Front|Off

			//设置深度测试时使用的函数
			ZTest Less Greater|LEqual|Gequal|Equal|NotEqual|Always

			//开启关闭深度写入
			ZWrite On|Off

			//开启并设置混合模式
			Blend SrcFactor DstFactor

			//Pass 定义了一次完整的渲染流程，因此Pass数目尽量小
			Pass
			{
				[Name]
				[Tags]
				[RenderSetup]
			}

			// OtherPass
		}	
	*/

	SubShader
	{
		Tags{"RenderType" = "Opaque"}
		CGPROGRAM
		#pragma surface surf Lambert
		struct Input
		{
			float4 color : COLOR;
		};
		void surf(Input IN, inout SurfaceOutput o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}



	FallBack "Diffuse"
}

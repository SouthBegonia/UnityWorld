Shader "Custom/S1"
{
	//材质的属性
	Properties
	{
		//格式为: 名字 显示的名称 类型

		//数字与滑块
		_int("Int",int) = 2
		_Float("Float",Float) = 1.5
		_Range("Range",Range(0.0,5.0)) = 3.0

		//颜色与坐标
		_Color("Color", Color) = (1,1,1,1)
		_Vector("Vector",Vector) = (2,3,6,1)

		//纹理
		_2D("2D",2D) = "" {}
		_Cube("Cube",Cube) = "White" {}
		_3D("3D",3D) = "black" {}
	}

		//SubShader
		//{
		//    Tags { "RenderType"="Opaque" }
		//    LOD 200

		//    CGPROGRAM
		//    // Physically based Standard lighting model, and enable shadows on all light types
		//    #pragma surface surf Standard fullforwardshadows

		//    // Use shader model 3.0 target, to get nicer looking lighting
		//    #pragma target 3.0

		//    sampler2D _MainTex;

		//    struct Input
		//    {
		//        float2 uv_MainTex;
		//    };

		//    half _Glossiness;
		//    half _Metallic;
		//    fixed4 _Color;

		//    // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		//    // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		//    // #pragma instancing_options assumeuniformscaling
		//    UNITY_INSTANCING_BUFFER_START(Props)
		//        // put more per-instance properties here
		//    UNITY_INSTANCING_BUFFER_END(Props)

		//    void surf (Input IN, inout SurfaceOutputStandard o)
		//    {
		//        // Albedo comes from a texture tinted by color
		//        fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
		//        o.Albedo = c.rgb;
		//        // Metallic and smoothness come from slider variables
		//        o.Metallic = _Metallic;
		//        o.Smoothness = _Glossiness;
		//        o.Alpha = c.a;
		//    }
		//    ENDCG
		//}
		FallBack "Diffuse"
}

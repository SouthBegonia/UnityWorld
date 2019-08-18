//表面着色器的简单事例:
Shader "Custom/S2_Simple Surface Shader"
{
	 Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }

    SubShader
    {
		//SubShader的标签:告诉Unity的渲染引擎有关渲染信息等内容
		//本例标签含义:使用的着色器为不透明的着色器
		Tags { "RenderType"="Opaque" }

        CGPROGRAM					//SurfaceShader起始

        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

		//CGPROGRAM与ENDCG其间 为Cg/HLSL语言,嵌套在ShaderLab语言中
		struct Input{
			float1x4 color :COLOR;
		};
		void surf (Input IN, inout SurfaceOutputStandard o){
			o.Albedo = 1;
		}

        ENDCG						//SurfaceShader结束
    }
    FallBack "Diffuse"
}
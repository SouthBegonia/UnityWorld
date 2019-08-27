//定点/片元着色器
Shader "Custom/S3_SimpleVertexFragmentShader"
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
        Pass{
			CGPROGRAM

			#pragma vertex vert 
			#pragma fragment frag 

			float4 vert(float4 v : POSITION) : SV_POSITION {
				return UnityObjectToClipPos (v);
				}
			float4 frag() : SV_Target {
				return fixed4(1.0,0.0,0.0,1.0);
				}
			ENDCG
		}
	}
}

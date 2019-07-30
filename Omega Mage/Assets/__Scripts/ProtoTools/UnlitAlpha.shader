Shader "ProtoTools/UnlitAlpha"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans. (Alpha)", 2D) = "white" { }
    }

    Category
    {
        ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha
        SubShader
        {
        Tags { "Queue" = "Transparent" }
            Pass
            {
            	ZWrite Off
   			    Cull Off
                Lighting Off
				SetTexture [_MainTex]
				{
					constantColor [_Color]
					Combine texture * constant, texture * constant 
				} 
                
            }
        } 
    }
}

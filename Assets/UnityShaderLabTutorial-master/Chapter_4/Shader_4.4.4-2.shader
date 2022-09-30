﻿Shader "Custom/Color Property"
{
    Properties
    {
        // 开放颜色属性_MainColor
        _MainColor ("MainColor", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // 声明属性变量_MainColor
            fixed4 _MainColor;

            void vert (in float4 vertex : POSITION,
                    out float4 position : SV_POSITION)
            {
                position = UnityObjectToClipPos(vertex);
            }

            void frag (out fixed4 color : SV_TARGET)
            {
                // 调用颜色变量_MainColor
                color = _MainColor;
            }
            ENDCG
        }
    }
}

﻿Shader "Custom/Cubemap Property"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _MainColor ("MainColor", Color) = (1, 1, 1, 1)

        // 添加Cubemap属性和反射强度
        _Cubemap ("Cubemap", Cube) = "" {}
        _Reflection ("Reflection", Range(0, 1)) = 0
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler _MainTex;
            float4 _MainTex_ST;
            fixed4 _MainColor;

            // 声明Cubemap和反射属性变量
            samplerCUBE _Cubemap;
            fixed _Reflection;

            void vert (in float4 vertex : POSITION,
                        in float3 normal : NORMAL,
                        in float4 uv : TEXCOORD0,
                        out float4 position : SV_POSITION,
                        out float4 worldPos : TEXCOORD0,
                        out float3 worldNormal : TEXCOORD1,
                        out float2 texcoord : TEXCOORD2)
            {
                position = UnityObjectToClipPos(vertex);

                // 将顶点坐标变换到世界空间
                worldPos = mul(unity_ObjectToWorld, vertex);

                // 将法线向量变换到世界空间
                worldNormal = mul(normal, (float3x3)unity_WorldToObject);
                worldNormal = normalize(worldNormal);

                texcoord = uv * _MainTex_ST.xy + _MainTex_ST.zy;
            }

            void frag (in float4 position : SV_POSITION,
                        in float4 worldPos : TEXCOORD0,
                        in float3 worldNormal : TEXCOORD1,
                        in float2 texcoord : TEXCOORD2,
                        out fixed4 color : SV_Target)
            {
                fixed4 main = tex2D(_MainTex, texcoord) * _MainColor;

                // 计算世界空间中从摄像机指向顶点的方向向量
                float3 viewDir = worldPos.xyz - _WorldSpaceCameraPos;
                viewDir = normalize(viewDir);

                // 套用公式计算反射向量
                float3 refDir = 2 * dot(-viewDir, worldNormal)
                                * worldNormal + viewDir;
                refDir = normalize(refDir);

                // 对Cubemap采样
                fixed4 reflection = texCUBE(_Cubemap, refDir);

                // 使用_Reflection对颜色和反射进行线性插值计算
                color = lerp(main, reflection, _Reflection);
            }
            ENDCG
        }
    }
}

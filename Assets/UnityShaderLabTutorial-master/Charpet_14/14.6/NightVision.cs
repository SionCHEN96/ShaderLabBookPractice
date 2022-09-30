﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class NightVision : MonoBehaviour
{
    public Shader EffectShader;

    [Header("Basic Properties")]
    [Range(-2, 2)] public float Distortion = 0.5f;
    [Range(0.01f, 1)] public float Scale = 0.5f;
    [Range(-1, 1)] public float Brightness = 0;
    [Range(0, 2)] public float Saturation = 1;
    [Range(0, 2)] public float Contrasrt = 1;

    public Color Tint = Color.black;

    [Header("Advanced Properties")]
    [Range(0, 10)] public float VignetteFalloff = 1;
    [Range(0, 100)] public float VignetteIntensity = 1;

    public Texture2D Noise;
    [Range(0, 10)] public float NoiseAmount = 1;
    private float RandomValue;

    private Material currentMaterial;

    Material EffectMaterial
    {
        get
        {
            if (currentMaterial == null)
            {
                currentMaterial = new Material(EffectShader)
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
            }
            return currentMaterial;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (EffectMaterial != null)
        {
            EffectMaterial.SetFloat("_Distortion", Distortion);
            EffectMaterial.SetFloat("_Scale", Scale);

            EffectMaterial.SetFloat("_Brightness", Brightness);
            EffectMaterial.SetFloat("_Saturation", Saturation);
            EffectMaterial.SetFloat("_Contrast", Contrasrt);

            EffectMaterial.SetColor("_Tint", Tint);

            EffectMaterial.SetFloat("_VignetteFalloff", VignetteFalloff);
            EffectMaterial.SetFloat("_VignetteIntensity", VignetteIntensity);

            if (Noise != null)
            {
                EffectMaterial.SetTexture("_Noise", Noise);
                EffectMaterial.SetFloat("_NoiseAmount", NoiseAmount);
                EffectMaterial.SetFloat("_RandomValue", RandomValue);
            }

            Graphics.Blit(source, destination, EffectMaterial);
        }
        else Graphics.Blit(source, destination);
    }

    private void Update()
    {
        //随机生成范围中的数值
        RandomValue = Random.Range(-3.14f, 3.14f);
    }
}

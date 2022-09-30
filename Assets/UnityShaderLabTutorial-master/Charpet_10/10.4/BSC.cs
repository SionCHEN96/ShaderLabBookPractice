﻿using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(BSCRenderer), PostProcessEvent.AfterStack, "Custom/BSC")]
public sealed class BSC : PostProcessEffectSettings
{
    //开放属性
    [Range(0f, 2f), Tooltip("Brightness effect intensity.")]
    public FloatParameter Brightness = new FloatParameter { value = 1f };

    [Range(0f, 2f), Tooltip("Saturation effect intensity.")]
    public FloatParameter Saturation = new FloatParameter { value = 1f };

    [Range(0f, 2f), Tooltip("Contrast effect intensity.")]
    public FloatParameter Contrast = new FloatParameter { value = 1f };
}

public sealed class BSCRenderer : PostProcessEffectRenderer<BSC>
{
    public override void Render(PostProcessRenderContext context)
    {
        // 查找Shader文件
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/BSC-HLSL"));

        //传递属性到Shader
        sheet.properties.SetFloat("_Brightness", settings.Brightness);
        sheet.properties.SetFloat("_Saturation", settings.Saturation);
        sheet.properties.SetFloat("_Contrast", settings.Contrast);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}

using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PostProcessOutlineRenderer), PostProcessEvent.AfterStack, "Outline")]
public sealed class PostProcessOutline : PostProcessEffectSettings {
    public FloatParameter minDepth = new FloatParameter { value = 0f };
    public FloatParameter maxDepth = new FloatParameter { value = 1f };
    public FloatParameter thickness = new FloatParameter { value = 1f };
}

public sealed class PostProcessOutlineRenderer : PostProcessEffectRenderer<PostProcessOutline> {
    public static RenderTexture outlineRenderTexture;

    public override DepthTextureMode GetCameraFlags() {
        return DepthTextureMode.Depth;
    }

    void Start() {
        if (outlineRenderTexture == null || outlineRenderTexture.width != Screen.width ||
                outlineRenderTexture.height != Screen.height) {
            outlineRenderTexture = new RenderTexture(Screen.width, Screen.height, 0);
        }
    }

    public override void Render(PostProcessRenderContext context) {
        var sheet = context.propertySheets.Get(Shader.Find("Custom/Outline"));
        sheet.properties.SetFloat("_MinDepth", settings.minDepth);
        sheet.properties.SetFloat("_MaxDepth", settings.maxDepth);
        sheet.properties.SetFloat("_Thickness", settings.thickness);

        if (outlineRenderTexture == null || outlineRenderTexture.width != Screen.width ||
                outlineRenderTexture.height != Screen.height) {
            outlineRenderTexture = new RenderTexture(Screen.width, Screen.height, 0);
        }
        context.camera.targetTexture = outlineRenderTexture;

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}

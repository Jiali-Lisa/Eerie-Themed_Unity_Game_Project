using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


[Serializable]
[PostProcess(typeof(PostProcessOutlineCompositeRenderer), PostProcessEvent.AfterStack, "Outline Composite")]
public sealed class PostProcessOutlineComposite : PostProcessEffectSettings {
    public ColorParameter color = new ColorParameter() {
        value = Color.black
    };
}

public class PostProcessOutlineCompositeRenderer : PostProcessEffectRenderer<PostProcessOutlineComposite> {
    public override void Render(PostProcessRenderContext context) {
        var sheet = context.propertySheets.Get(Shader.Find("Custom/OutlineComposite"));
        sheet.properties.SetColor("_Color", settings.color);

        if (PostProcessOutlineRenderer.outlineRenderTexture != null) {
            sheet.properties.SetTexture("_OutlineTexture", PostProcessOutlineRenderer.outlineRenderTexture);
        }

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}

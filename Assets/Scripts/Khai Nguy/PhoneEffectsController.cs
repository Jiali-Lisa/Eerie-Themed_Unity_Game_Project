using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneEffectsController : MonoBehaviour
{
    public Shader postShader;
    Material postEffectMat;

    private enum GreyScale {
        Off,
        On,
    }

    [SerializeField] private Color color;
    [SerializeField] private GreyScale greyScale;

    void Awake() {
        if (postEffectMat == null) {
            postEffectMat = new Material(postShader);
        }

        postEffectMat.SetColor("_Color", color);
    }

    public void changeColor(Color newColor) {
        color = newColor;
        postEffectMat.SetColor("_Color", color);
    }

    public void setGreyScale(bool value) {
        if (value) greyScale = GreyScale.On;
        if (!value) greyScale = GreyScale.Off;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst) {
        RenderTexture texture = RenderTexture.GetTemporary(
                src.width,
                src.height,
                0,
                src.format
            );

        Graphics.Blit(src, texture);

        if (greyScale == GreyScale.On) {
            RenderTexture tempTex = RenderTexture.GetTemporary(
                    src.width,
                    src.height,
                    0,
                    src.format
                );
            Graphics.Blit(texture, tempTex, postEffectMat, 0);
            Graphics.Blit(tempTex, texture);
            RenderTexture.ReleaseTemporary(tempTex);
        }

        Graphics.Blit(texture, dst);

        RenderTexture.ReleaseTemporary(texture);
    }
}

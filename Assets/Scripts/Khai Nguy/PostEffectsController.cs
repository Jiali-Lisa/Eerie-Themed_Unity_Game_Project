using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class PostEffectsController : MonoBehaviour
{
    private Shader postShader;
    Material postEffectMat;

    private enum Distort {
        Off,
        On,
    }

    private enum Vignette {
        Off,
        On,
    }

    [SerializeField] private Vignette vignette = Vignette.Off;
    [SerializeField] private float radius = 0.8f;
    [SerializeField] private float feather = 0.8f;
    [SerializeField] private float frequency = 1;
    [SerializeField] private Color tint;
    [SerializeField] private Distort distort = Distort.Off;
    [SerializeField] private float distortTimeScale = 2;
    [SerializeField] private float distortScale = 1;

    void Awake() {
        postShader = Shader.Find("Custom/PostProcessing");
    }

    void Start() {
        if (postEffectMat == null) {
            postEffectMat = new Material(postShader);
        }

        postEffectMat.SetFloat("_Radius", radius);
        postEffectMat.SetFloat("_Feather", feather);
        postEffectMat.SetFloat("_Frequency", frequency);
        postEffectMat.SetColor("_Tint", tint);
        postEffectMat.SetFloat("_TimeScale", distortTimeScale);
        postEffectMat.SetFloat("_Scale", distortScale);
    }

    public void changeVignetteRadius(float newRadius) {
        radius = newRadius;
        postEffectMat.SetFloat("_Radius", radius);
    }

    public void decreaseVignetteRadius(float decreaseBy) {
        radius -= decreaseBy;
        postEffectMat.SetFloat("_Radius", radius);
    }

    public void changeVignetteFeather(float newFeather) {
        feather = newFeather;
        postEffectMat.SetFloat("_Feather", feather);
    }

    public void changeVignetteFrequency(float newFreq) {
        frequency = newFreq;
        postEffectMat.SetFloat("_Frequency", frequency);
    }

    public void changeVignetteTint(Color newTint) {
        tint = newTint;
        postEffectMat.SetColor("_Tint", tint);
    }

    public void setVignette(bool value) {
        if (value) vignette = Vignette.On;
        if (!value) vignette = Vignette.Off;
    }

    public void setDistort(bool value) {
        if (value) distort = Distort.On;
        if (!value) distort = Distort.Off;
    }

    public void setDistortTimeScale(float value) {
        postEffectMat.SetFloat("_TimeScale", distortTimeScale);
    }

    public void setDistortScale(float value) {
        postEffectMat.SetFloat("_Scale", distortScale);
    }

    public void allOff() {
        vignette = Vignette.Off;
        distort = Distort.Off;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst) {
        if (postEffectMat == null) {
            postEffectMat = new Material(postShader);
            Debug.Log("No Material");
        }

        RenderTexture texture = RenderTexture.GetTemporary(
                src.width,
                src.height,
                0,
                src.format
            );

        Graphics.Blit(src, texture);

        if (vignette == Vignette.On) {
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

        if (distort == Distort.On) {
            RenderTexture tempTex = RenderTexture.GetTemporary(
                    src.width,
                    src.height,
                    0,
                    src.format
                );
            Graphics.Blit(texture, tempTex, postEffectMat, 1);
            Graphics.Blit(tempTex, texture);
            RenderTexture.ReleaseTemporary(tempTex);
        }

        Graphics.Blit(texture, dst);

        RenderTexture.ReleaseTemporary(texture);
    }
}

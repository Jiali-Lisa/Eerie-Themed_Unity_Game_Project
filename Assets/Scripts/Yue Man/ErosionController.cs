using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErosionController : MonoBehaviour
{
    public Material erosionMaterial;  // Assign the material using your shader
    public float revealSpeed = 0.5f;  // Speed of the erosion effect
    private float currentRevealValue = 0f;

    void Update()
    {
        // Increase the reveal value over time
        currentRevealValue += Time.deltaTime * revealSpeed;

        // Clamp the reveal value to stay between 0 and 1
        currentRevealValue = Mathf.Clamp01(currentRevealValue);

        // Update the material's _RevealValue property
        erosionMaterial.SetFloat("_RevealValue", currentRevealValue);
    }
}

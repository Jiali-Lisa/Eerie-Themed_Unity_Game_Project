using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangerWithClickAndReset : MonoBehaviour
{
    public Renderer objectRenderer;
    public Color[] colors;
    public float[] timePoints;
    public float resetDelay; 

    private Color originalColor;
    public bool isColorChanged = false;
    private bool isColorChanging = false; 
    private int currentIndex = 0;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.material = new Material(objectRenderer.material); 
        originalColor = objectRenderer.material.color;
        resetDelay = Random.Range(1.0f, 3.0f);
    }

    public void TriggerColorChange()
    {
        if (!isColorChanged) 
        {
            StartCoroutine(ChangeColorOverTime());
        }
    }

    IEnumerator ChangeColorOverTime()
    {
        isColorChanging = true;
        for (int i = 0; i < colors.Length; i++)
        {
            yield return new WaitForSeconds(timePoints[i] - (i > 0 ? timePoints[i - 1] : 0));
            objectRenderer.material.color = colors[i];
            isColorChanged = true;
            currentIndex = i;
        }
        isColorChanging = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform && isColorChanged)
                {
                    objectRenderer.material.color = originalColor;
                    isColorChanged = false;
                    StartCoroutine(RestartColorChangeAfterDelay());
                }
            }
        }
    }


    IEnumerator RestartColorChangeAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);
        TriggerColorChange();
    }

    public void ResetToOriginalColor()
    {
        objectRenderer.material.color = originalColor; 
        isColorChanged = false; 
    }
}
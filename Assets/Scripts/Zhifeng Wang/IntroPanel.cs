using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPanel : MonoBehaviour
{
    public GameObject panel;

    void Start()
    {
        Invoke("HidePanel", 2f);
    }

    void HidePanel()
    {
        panel.SetActive(false);
    }
}

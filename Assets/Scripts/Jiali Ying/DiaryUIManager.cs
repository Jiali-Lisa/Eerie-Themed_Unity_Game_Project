using UnityEngine;
using TMPro;

public class DiaryUIManager : MonoBehaviour
{
    public TMP_Text interactText; 

    public void HideInteractText()
    {
        if (interactText != null)
        {
            interactText.text = ""; 
            interactText.gameObject.SetActive(false); 
        }
    }
}


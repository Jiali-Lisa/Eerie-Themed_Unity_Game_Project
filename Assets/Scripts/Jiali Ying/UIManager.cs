using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject hint; 

    public void ShowDialogue()
    {
        hint.SetActive(false);
    }

    public void HideDialogue()
    {
        hint.SetActive(true);
    }
}

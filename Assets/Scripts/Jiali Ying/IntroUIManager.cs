using UnityEngine;

public class IntroUIManager : MonoBehaviour
{
    public GameObject doorHint;
    //public GameObject bookHint; 

    public void ShowDialogue()
    {
        doorHint.SetActive(false);
        //bookHint.SetActive(false);
    }

    public void HideDialogue()
    {
        doorHint.SetActive(true);
        //bookHint.SetActive(true);
    }
}
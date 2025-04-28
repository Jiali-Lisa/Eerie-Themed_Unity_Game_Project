using UnityEngine;

public class IntroDialogueTrigger : MonoBehaviour
{
    public IntroDialogueManager dialogueManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.StartDialogue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.HideDialogue();
        }
    }
}
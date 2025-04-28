using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;  
    [SerializeField] private DialogueData dialogueData;        
    public System.Action onDialogueEnd;
    public DialogueManager.DialogueType dialogueType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger Entered: " + gameObject.name);
            if (dialogueManager != null && dialogueData != null)
            {
                GetComponent<Collider>().enabled = false;
                dialogueManager.StartDialogue(dialogueData, dialogueType, onDialogueEnd);
                Debug.Log("Dialogue played");
            }
            else
            {
                Debug.LogWarning("DialogueManager or DialogueData is not set!");
            }
        }
    }
}

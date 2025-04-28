using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject playerDialoguePanel;
    [SerializeField] private GameObject npcDialoguePanel;
    [SerializeField] private PlayerMovement playerMovement; 
    [SerializeField] private TMP_Text qText;
    [SerializeField] private TMP_Text playerDialogueText; 
    [SerializeField] private TMP_Text npcDialogueText;
    [SerializeField] private Rigidbody playerRigidbody;

    private string[] dialogues;
    private int currentDialogueIndex = 0;
    private System.Action onDialogueEnd;
    private DialogueType currentDialogueType;
    private PauseMenu pausemenu;

    public enum DialogueType
    {
        Player,
        NPC
    }

    private void Start()
    {
        playerDialoguePanel.SetActive(true);  
        npcDialoguePanel.SetActive(false);
        playerMovement.enableControl(false); 

        pausemenu = FindObjectOfType<PauseMenu>();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShowNextDialogue();
        }
    }

    public void StartDialogue(DialogueData newDialogueData, DialogueType dialogueType, System.Action callback = null)
    {
        dialogues = newDialogueData.dialogues;
        onDialogueEnd = callback;
        currentDialogueIndex = 0;
        currentDialogueType = dialogueType;

        pausemenu.setdialogue(true);

        if (dialogueType == DialogueType.NPC)
        {
            playerDialoguePanel.SetActive(false);
            npcDialoguePanel.SetActive(true);
        }
        else
        {
            playerDialoguePanel.SetActive(true);
            npcDialoguePanel.SetActive(false);
        }

        ShowDialogue(currentDialogueIndex);
        playerMovement.enableControl(false);
        StopPlayerMovement();
        uiManager.ShowDialogue();
    }

    public void ShowNextDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex < dialogues.Length)
        {
            ShowDialogue(currentDialogueIndex);
        }
        else
        {
            HideDialogue();
        }
    }

    public void HideDialogue()
    {
        playerDialogueText.text = "";
        npcDialogueText.text = "";
        qText.text = "";
        playerDialoguePanel.SetActive(false);
        npcDialoguePanel.SetActive(false);
        playerMovement.enableControl(true);
        uiManager.HideDialogue();
        pausemenu.setdialogue(false);

        onDialogueEnd?.Invoke();
    }

    private void ShowDialogue(int index)
    {
        qText.text = "Press Q▼";

        if (currentDialogueType == DialogueType.NPC)
        {
            npcDialogueText.text = dialogues[index];
            npcDialogueText.color = Color.red;
            qText.color = Color.black;
        }
        else
        {
            playerDialogueText.text = dialogues[index];  
            playerDialogueText.color = Color.white;
            qText.color = Color.white;
        }
    }

    private void StopPlayerMovement()
    {
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
    }
}

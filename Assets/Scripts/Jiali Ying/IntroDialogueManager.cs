using UnityEngine;
using TMPro;

public class IntroDialogueManager : MonoBehaviour
{
    [SerializeField] private IntroUIManager uiManager;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private PlayerMovement playerMovement; 
    [SerializeField] private TMP_Text qText;
    [SerializeField] private Rigidbody playerRigidbody;

    private PauseMenu pausemenu;

    private string[] dialogues = {
        "How can I left my notebook at school?",
        "School at night is so scary.",
        "I shouldn't spend too long here.",
        "I'd better go to the classroom, grab my notebook and leave."
    };

    private int currentDialogueIndex = 0;

    private void Start()
    {
        pausemenu = FindObjectOfType<PauseMenu>();
        dialoguePanel.SetActive(true);
        ShowDialogue(currentDialogueIndex);
        playerMovement.enableControl(false); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShowNextDialogue();
        }
    }

    public void StartDialogue()
    {
        pausemenu.setdialogue(true);
        currentDialogueIndex = 0;
        dialoguePanel.SetActive(true);
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
        pausemenu.setdialogue(false);
        dialogueText.text = "";
        qText.text = "";
        dialoguePanel.SetActive(false);
        playerMovement.enableControl(true);
        uiManager.HideDialogue();
    }

    private void ShowDialogue(int index)
    {
        dialogueText.text = dialogues[index];
    }

    private void StopPlayerMovement()
    {
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
    }
}
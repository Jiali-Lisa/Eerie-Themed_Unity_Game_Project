using UnityEngine;
using TMPro;

public class FirstDiaryDialogueManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private TMP_Text qText;
    [SerializeField] private Rigidbody playerRigidbody;

    private PauseMenu pausemenu;

    private System.Action onDialogueEnd;

    private string[] dialogues;
    private int currentDialogueIndex = 0;

    private void Start()
    {
        dialoguePanel.SetActive(true);
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

    public void StartDialogue(DialogueData newDialogueData, System.Action onDialogueEnd = null)
    {
        dialogues = newDialogueData.dialogues;
        currentDialogueIndex = 0;
        dialoguePanel.SetActive(true);
        ShowDialogue(currentDialogueIndex);
        playerMovement.enableControl(false);
        StopPlayerMovement();
        uiManager.ShowDialogue();

        pausemenu.setdialogue(true);

        this.onDialogueEnd = onDialogueEnd;
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
        dialogueText.text = "";
        qText.text = "";
        dialoguePanel.SetActive(false);
        playerMovement.enableControl(true);
        uiManager.HideDialogue();

        pausemenu.setdialogue(false);

        onDialogueEnd?.Invoke();
    }

    private void ShowDialogue(int index)
    {
        qText.text = "Press Q▼";
        dialogueText.text = dialogues[index];
    }

    private void StopPlayerMovement()
    {
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
    }
}
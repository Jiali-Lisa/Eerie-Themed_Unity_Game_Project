using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChasingBridgeDialogueManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private TMP_Text qText;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private DialogueData Bridge;

    [SerializeField] private Image blackScreenImage;
    [SerializeField] private CanvasGroup blackScreenCanvasGroup;

    private PauseMenu pausemenu;

    private string[] dialogues;

    private int currentDialogueIndex = 0;

    private void Start()
    {
        dialoguePanel.SetActive(true);
        playerMovement.enableControl(false);

        pausemenu = FindObjectOfType<PauseMenu>();
        if (Bridge != null && Bridge.dialogues.Length > 0)
        {
            StartDialogue(Bridge);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShowNextDialogue();
        }
    }

    public void StartDialogue(DialogueData newDialogueData)
    {
        dialogues = newDialogueData.dialogues;
        currentDialogueIndex = 0;
        dialoguePanel.SetActive(true);
        ShowDialogue(currentDialogueIndex);
        playerMovement.enableControl(false);
        StopPlayerMovement();
        uiManager.ShowDialogue();
        pausemenu.setdialogue(true);
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
        uiManager.HideDialogue();
        pausemenu.setdialogue(false);
        StartCoroutine(FadeToBlackAndSwitchScene());
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

    private IEnumerator FadeToBlackAndSwitchScene()
    {
        blackScreenImage.gameObject.SetActive(true);
        blackScreenCanvasGroup.alpha = 0;
        float duration = 1.0f;
        float waitDuration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            blackScreenCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }

        yield return new WaitForSeconds(waitDuration);

        SceneManager.LoadScene("Chasing");
    }
}
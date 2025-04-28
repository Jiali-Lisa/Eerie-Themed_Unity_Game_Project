using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class EndingDialogueManager : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text qText;

    [SerializeField] private Image blackScreenImage;
    [SerializeField] private CanvasGroup blackScreenCanvasGroup;

    private string[] dialogues = {
        "You finally got the right answers.",
        "Mental illness is not something to fear.",
        "Face it with courage, and you'l find the light at the end.",
        "Thank you for saving me.",
        "Thank you...",
        "For saving...",
        "YOURSELF",
        "Emily."
    };

    private int currentDialogueIndex = 0;

    private void Start()
    {
        dialoguePanel.SetActive(true);
        ShowDialogue(currentDialogueIndex);
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
        currentDialogueIndex = 0;
        dialoguePanel.SetActive(true);
        ShowDialogue(currentDialogueIndex);
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
        StartCoroutine(FadeToBlackAndSwitchScene());
    }

    private void ShowDialogue(int index)
    {
        dialogueText.text = dialogues[index];
        qText.color = Color.black;
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

        SceneManager.LoadScene("Subtitle");
    }
}
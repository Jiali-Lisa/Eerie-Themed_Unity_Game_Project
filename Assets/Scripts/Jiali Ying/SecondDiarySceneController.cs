using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SecondDiarySceneController : MonoBehaviour
{
    public FirstDiaryDialogueManager dialogueManager;
    public DialogueData startDiaryDialogue;
    public DialogueData afterDiaryDialogue;
    public PlayerMovement playerMovement;
    public GameObject bookImageUI;
    public Button quitButton;

    [SerializeField] private UIManager uiManager;

    [SerializeField] private Image blackScreenImage;
    [SerializeField] private CanvasGroup blackScreenCanvasGroup;

    private void Start()
    {
        bookImageUI.SetActive(false);
        quitButton.gameObject.SetActive(false);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
        dialogueManager.StartDialogue(startDiaryDialogue);
    }

    private void OnQuitButtonClicked()
    {
        HideBookImage();
        dialogueManager.StartDialogue(afterDiaryDialogue, () =>
        {
            uiManager.ShowDialogue();
            StartCoroutine(FadeToBlackAndSwitchScene());
        });

    }

    private void HideBookImage()
    {
        bookImageUI.SetActive(false);
        quitButton.gameObject.SetActive(false);
        playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

        SceneManager.LoadScene("ChaseBridge");
    }
}

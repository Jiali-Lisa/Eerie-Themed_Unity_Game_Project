using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public DialogueData gameplayIntroDialogue;
    public DialogueData npcDialogueAtDoor;
    public DialogueData playerDialogueAfterNpc;
    public DialogueData teacherDialogueAtDesk;
    public DialogueData teacherDialogueAfterBook;

    public DialogueTrigger doorDialogueTrigger;

    public PlayerMovement playerMovement;
    public Transform playerTransform;
    public Transform teacherTransform;

    [SerializeField] private Image blackScreenImage;
    [SerializeField] private CanvasGroup blackScreenCanvasGroup;

    private void Start()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.hasSavedState)
        {
            GameStateManager.Instance.RestorePlayerState(playerTransform);

            Vector3 direction = (teacherTransform.position - playerTransform.position).normalized;
            float lookUpAngle = 15f;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Quaternion lookUpRotation = Quaternion.Euler(lookRotation.eulerAngles.x - lookUpAngle, lookRotation.eulerAngles.y, lookRotation.eulerAngles.z);
            playerTransform.rotation = lookUpRotation;

            Cursor.lockState = CursorLockMode.Locked;  
            Cursor.visible = false;

            TeacherDialogueAfterBook();

        }
        else
        {
            dialogueManager.StartDialogue(gameplayIntroDialogue, DialogueManager.DialogueType.Player, () =>
            {

                doorDialogueTrigger.onDialogueEnd = PlayerSelfDialogueAfterNpc;
                doorDialogueTrigger.enabled = true;
            });


        }


    }

    private void PlayerSelfDialogueAfterNpc()
    {
        playerMovement.enableControl(false); 
        dialogueManager.StartDialogue(playerDialogueAfterNpc, DialogueManager.DialogueType.Player, () => {

            playerMovement.enableControl(true); 
        });
    }


    public void TeacherDialogueAfterBook()
    {
        playerMovement.enableControl(false);
        Debug.Log("Desk Dialogue Trigger Entered");
        dialogueManager.StartDialogue(teacherDialogueAfterBook, DialogueManager.DialogueType.NPC, () => {
            StartCoroutine(FadeToBlackAndSwitchScene());
        });
    }

    public void SavePlayerStateBeforeSceneChange()
    {
        GameStateManager.Instance.SavePlayerState(playerTransform);
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

        SceneManager.LoadScene("FirstDiary");
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public enum DIALOGUES {
    START,
    DOOR,
    DISTORT1,
    DISTORT2,
    FLASH,
    LOCKER,
    EXIT
}

public class ChasingDialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject Q;
    [SerializeField] private GameObject player;
    private Rigidbody playerBody;
    private PlayerMovement playerMovement;
    private PlayerInteract playerInteract;
    private bool allowMove = false;
    private bool allowCameraMove = false;

    private Dictionary<DIALOGUES, string[]> dialogues = new Dictionary<DIALOGUES, string[]>();
    private string[] startDialogues = {
        "I want my medicine! I want to go back to the reality! Where? Where am I?",
        "What is this place?",
    };
    private string[] doorDialogues = {
        "Hmm? The door is locked.",
    };
    private string[] distort1Dialogues = {
        "Is everything getting more disorienting?!",
        "I need to get out of here quickly!"
    };
    private string[] distort2Dialogues = {
        "Why is everything getting even more disorienting?!",
        "I need to find the way out quickly and get out!",
    };
    private string[] flashDialogues = {
        "It seems the flash can push it back!",
    };
    private string[] lockerDialogues = {
        "That locker seems different. Let's check it out!",
    };
    private string[] exitDialogues = {
        "I think I need to get through here.",
        "How can I open this?",
        "There maybe some kind of triggers to open it."
    };

    private string[] currentDialogues;
    private int currentDialogueIndex = 0;

    private void Start()
    {
        dialogues.Add(DIALOGUES.START, startDialogues);
        dialogues.Add(DIALOGUES.DOOR, doorDialogues);
        dialogues.Add(DIALOGUES.DISTORT1, distort1Dialogues);
        dialogues.Add(DIALOGUES.DISTORT2, distort2Dialogues);
        dialogues.Add(DIALOGUES.FLASH, flashDialogues);
        dialogues.Add(DIALOGUES.LOCKER, lockerDialogues);
        dialogues.Add(DIALOGUES.EXIT, exitDialogues);

        currentDialogues = dialogues[DIALOGUES.START];
        dialoguePanel.SetActive(true);
        Q.SetActive(true);
        ShowDialogue(currentDialogueIndex);

        playerBody = player.GetComponent<Rigidbody>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerInteract = player.GetComponent<PlayerInteract>();
        playerMovement.enableControl(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !allowMove && !isHidden())
        {
            ShowNextDialogue();
        }
    }

    public void StartDialogue()
    {
        currentDialogueIndex = 0;
        dialoguePanel.SetActive(true);

        if (!allowMove) {
            Q.SetActive(true);
            UI.SetActive(false);
            ShowDialogue(currentDialogueIndex);
            playerMovement.enableControl(allowMove, allowCameraMove);
            StopPlayerMovement();
        } else {
            Q.SetActive(false);
            StartCoroutine(ShowDialogues());
            setAllowInteract(false);
        }
    }

    private IEnumerator ShowDialogues() {
        foreach (string dialogue in currentDialogues) {
            dialogueText.text = dialogue;
            yield return new WaitForSeconds(4);
        }
        HideDialogue();
    }

    public void ShowNextDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex < currentDialogues.Length)
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
        if (currentDialogues == dialogues[DIALOGUES.START]) {
            gameObject.GetComponent<ChasingGameController>().stopDisorientation();
        }
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
        Q.SetActive(false);
        UI.SetActive(true);
        playerMovement.enableControl(true);
    }

    private void ShowDialogue(int index)
    {
        dialogueText.text = currentDialogues[index];
    }

    private void StopPlayerMovement()
    {
        if (!allowMove) playerBody.velocity = Vector3.zero;
        if (!allowCameraMove) playerBody.angularVelocity = Vector3.zero;
    }

    public void changeDialoguesSection(DIALOGUES section) {
        currentDialogues = dialogues[section];
        currentDialogueIndex = 0;
    }

    public void setAllowInteract(bool value) {
        allowMove = value;
        allowCameraMove = value;
    }

    public void setAllowInteract(bool move, bool camera) {
        allowMove = move;
        allowCameraMove = camera;
    }

    public bool isHidden() {
        return dialogueText.text == "";
    }
}

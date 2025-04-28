using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    private Animator animator;
    private Renderer doorRenderer;
    private bool isOpen = false;
    private bool isLocked = false;
    private bool isInteracted = false;
    // private bool isPlayerNearby = false;

    [SerializeField] private PlayAudio[] Audio;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioSource lockedAudio;

    private TaskManager taskManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        doorRenderer = GetComponentInChildren<Renderer>();

        taskManager = FindObjectOfType<TaskManager>();
        if (taskManager == null)
        {
            Debug.Log("TaskManager not found in the scene.");
        }
    }

    // Update is called once per frame
    public void Interact(GameObject player)
    {
        isInteracted = true;
        if (isLocked) {
            if (lockedAudio != null) lockedAudio.Play();
            return;
        }

        if (!isOpen)
        {
            animator.SetTrigger("Open");
            isOpen = true;

            if (Audio != null) {
                foreach (PlayAudio audio in Audio) {
                    audio.Play();
                }
            }
            if (taskManager != null)
            {
                taskManager.CompleteTask("Go to the classroom");
                taskManager.SetTask("Open the book");
            }
        }
        else
        {
            animator.SetTrigger("Close");
            isOpen = false;

            if (Audio != null) {
                foreach (PlayAudio audio in Audio) {
                    audio.Play();
                }
            }

        }

        if (doorRenderer != null)
        {
            doorRenderer.transform.position = transform.position;
            doorRenderer.transform.rotation = transform.rotation;
        }
    }

    public void InteractionOff() {}

    public string GetInteractText() {
        return isLocked ? "Door locked" : isOpen ? "Press E to close the door" : "Press E to open the door";
    }

    public Transform GetTransform() {
        return transform;
    }

    public bool IsOpen() {
        return isOpen;
    }

    public void LockDoor() {
        isLocked = true;
    }

    public void UnlockDoor() {
        isLocked = false;
    }

    public bool isDoorLocked() {
        return isLocked;
    }

    public bool IsInteracted() {
        return isInteracted;
    }

    public void setInteracted(bool value) {
        isInteracted = value;
    }
    //
    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject == player)
    //     {
    //         isPlayerNearby = true;
    //     }
    // }
    //
    // void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject == player)
    //     {
    //         isPlayerNearby = false;
    //     }
    // }
}

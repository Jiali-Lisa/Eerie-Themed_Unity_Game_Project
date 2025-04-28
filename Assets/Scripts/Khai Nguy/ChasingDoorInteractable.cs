using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChasingDoorInteractable : MonoBehaviour, IInteractable
{
    private Animator animator;
    private Renderer doorRenderer;
    private bool isOpen = false;
    private bool isLocked = false;
    private bool isInteracted = false;

    [SerializeField] private PlayAudio[] Audio;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioSource lockedAudio;
    [SerializeField] private GameObject wall;
    [SerializeField] private TriggerInteractable trigger;
    private bool isInDefaultLayer;

    void Start()
    {
        animator = GetComponent<Animator>();
        doorRenderer = GetComponentInChildren<Renderer>();
        isInDefaultLayer = gameObject.layer == LayerMask.NameToLayer("Default");
    }

    public void Interact(GameObject player)
    {
        if (!isInteracted && !isInDefaultLayer) {
            changeLayer();
        }
        isInteracted = true;

        if (isLocked) {
            if (lockedAudio != null) lockedAudio.Play();
            trigger.AddTask(TaskType.UNLOCK);
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
        return isLocked ? "Locked" : isOpen ? "Close" : "Open";
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

    public void changeLayer() {
        if (wall == null) {
            return;
        }
        wall.transform.GetChild(0).gameObject.SetActive(false);
        wall.layer = LayerMask.NameToLayer("Default");
        gameObject.layer = LayerMask.NameToLayer("Default");
        foreach (Transform child in transform) {
            child.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}

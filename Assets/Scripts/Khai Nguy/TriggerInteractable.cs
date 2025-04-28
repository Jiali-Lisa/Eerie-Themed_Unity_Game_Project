using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioSource interactAudio;
    [SerializeField] private ChasingTasksManager tasksManager;
    private bool isInteracted = false;
    private TaskType taskType;

    // Update is called once per frame
    public void Interact(GameObject player)
    {
        if (!isInteracted) {
            isInteracted = true;
            interactAudio.Play();
        }
        tasksManager.CompleteTask(this.taskType, this);
    }

    public void InteractionOff() {}

    public string GetInteractText() {
        return isInteracted ? "" : "Trigger";
    }

    public Transform GetTransform() {
        return transform;
    }

    public bool IsInteracted() {
        return isInteracted;
    }

    public void AddTask(TaskType type) {
        tasksManager.AddTask(type, this);
        this.taskType = type;
    }
}

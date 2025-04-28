using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    [SerializeField] private GameObject DialogueBox;

    public void Interact(GameObject player)
    {
        // 这里可以写对话框
        Debug.Log("Interact-NPC!");
        DialogueBox.SetActive(true);
    }

    public void InteractionOff() {
        DialogueBox.SetActive(false);
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}

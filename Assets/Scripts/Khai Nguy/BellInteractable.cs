using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellInteractable : MonoBehaviour, IInteractable
{

    [SerializeField] private AudioSource speaker;
    public void Interact(GameObject player)
    {
        Debug.Log("Interact Bell!");
        speaker.Play();
    }

    public void InteractionOff() {}

    public string GetInteractText()
    {
        return "Hit bell";
    }

    public Transform GetTransform()
    {
        return transform;
    }
}

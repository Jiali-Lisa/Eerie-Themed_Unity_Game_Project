using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingGameGoal : MonoBehaviour, IInteractable
{
    private bool interacted;
    // Start is called before the first frame update
    void Start()
    {
        interacted = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact(GameObject player)
    {
        interacted = true;
    }

    public void InteractionOff() {}

    public string GetInteractText()
    {
        return interacted ? "" : "Interact!";
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public bool isInteracted() {
        return interacted;
    }
}

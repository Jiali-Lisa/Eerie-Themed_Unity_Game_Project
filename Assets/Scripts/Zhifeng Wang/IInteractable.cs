using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact(GameObject player);
    void InteractionOff();
    string GetInteractText();
    Transform GetTransform();
    bool canPickUp() {
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemInteractable : MonoBehaviour, IInteractable
{
    public bool isUsing;
    public bool allowMouseLeft = false;
    public bool allowR = false;
    public bool interacted = false;

    public abstract void Interact(GameObject player);

    public abstract void InteractionOff();

    public abstract string GetInteractText();

    public abstract Transform GetTransform();

    public bool canPickUp() {
        return true;
    }

    public void setUsing(bool use) {
        isUsing = use;
    }

    public bool isAllowedR() {
        return allowR;
    }
    public bool isAllowedMouseLeft() {
        return allowMouseLeft;
    }
    public abstract void UseR();
    public abstract void UseMouseLeft();
    public void setInteracted() {
        this.interacted = true;
    }
    public bool isInteracted() {
        return this.interacted;
    }
}

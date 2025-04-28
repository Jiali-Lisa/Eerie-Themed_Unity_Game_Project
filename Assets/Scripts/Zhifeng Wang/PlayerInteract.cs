using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private IInteractable interactingObject = null;
    private Camera cam;
    [SerializeField] private Transform ItemHolder;
    private ItemInteractable item;
    public float pickUpRange = 2.5f; //how far the player can pickup the object from
    private bool usingItem = false;

    [SerializeField] private LayerMask excludedLayers;

    [SerializeField] private GameObject UIContainer;
    [SerializeField] private GameObject EButton;
    [SerializeField] private GameObject RightButton;
    [SerializeField] private GameObject LeftButton;
    [SerializeField] private GameObject RButton;

    private Animator EAnimation;
    private Animator RightAnimation;
    private Animator LeftAnimation;
    private Animator RAnimation;

    private Animator itemAnimation;

    private bool allowInteract = true;


    private void Start() {
        cam = GetComponentInChildren<Camera>();
        if (UIContainer != null) {
            UIContainer.SetActive(false);
        }

        if (ItemHolder != null) {
            itemAnimation = ItemHolder.gameObject.GetComponent<Animator>();
        }
        if (EButton != null) {
            EAnimation = EButton.GetComponentInChildren<Animator>();
        }
        if (RightButton != null) {
            RightAnimation = RightButton.GetComponentInChildren<Animator>();
        }
        if (LeftButton != null) {
            LeftAnimation = LeftButton.GetComponentInChildren<Animator>();
        }
        if (RButton != null) {
            RAnimation = RButton.GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        if (!allowInteract) {
            return;
        }

        if (item != null && item.isAllowedMouseLeft()) {
            if (Input.GetMouseButtonDown(0) && LeftButton != null) {
                LeftAnimation.SetTrigger("Press");
                item.UseMouseLeft();
            }
            if (Input.GetMouseButtonUp(0) && LeftButton != null) {
                LeftAnimation.SetTrigger("Release");
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            if (RightButton != null) {
                RightAnimation.SetTrigger("Press");
            }
            if (item != null) {
                if (usingItem) {
                    StartCoroutine(putAwayItem());
                } else {
                    StartCoroutine(useItem());
                }
            }
        }

        if (Input.GetMouseButtonUp(1)) {
            if (RightButton != null) {
                RightAnimation.SetTrigger("Release");
            }
        }

        if (item != null && item.isAllowedR()) {
            if (Input.GetKeyDown(KeyCode.R)) {
                if (RButton != null) {
                    RAnimation.SetTrigger("Press");
                }
                if (item != null && usingItem) {
                    item.UseR();
                }
            }

            if (Input.GetKeyUp(KeyCode.R)) {
                if (RButton != null) {
                    RAnimation.SetTrigger("Release");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            if (EButton != null) {
                EAnimation.SetTrigger("Press");
            }
            IInteractable interactable = GetInteractableObject();

            if (interactable != interactingObject)
            {
                if (interactingObject != null) {
                    interactingObject.InteractionOff();
                }

                interactingObject = interactable;
                if (interactable == null) {
                    return;
                }

                if (interactable.canPickUp()) {
                    ItemInteractable interactingItem = (ItemInteractable)interactable;

                    item = interactingItem.transform.gameObject.GetComponent<ItemInteractable>();
                    item.transform.parent = ItemHolder.transform;
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localRotation = new Quaternion(0, 0, 0, 1);
                    item.gameObject.layer = LayerMask.NameToLayer("Item");

                    usingItem = false;
                    item.Interact(gameObject);
                    item.setUsing(usingItem);
                    itemAnimation.SetTrigger("PutAway");
                    // item.gameObject.SetActive(usingItem);
                    UIContainer.SetActive(true);

                    return;
                }
                interactable.Interact(gameObject);
            }
            else if (interactingObject != null)
            {
                interactingObject.Interact(gameObject);
            }
        }

        if (Input.GetKeyUp(KeyCode.E)) {
            if (EButton != null) {
                EAnimation.SetTrigger("Release");
            }
        }

        // item not allowed while sprint/dash
        if (GetComponent<PlayerMovement>().usingMovementAbility()) {
            if (usingItem) StartCoroutine(putAwayItem());
        }
    }

    public IInteractable GetInteractableObject()
    {
        List<IInteractable> interactableList = new List<IInteractable>();

        float interactRange = 2.5f;
        LayerMask layers = ~(excludedLayers | GetComponent<Rigidbody>().excludeLayers);
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange, layers);
        foreach (Collider collider in colliderArray){
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                Vector3 viewPos = cam.WorldToViewportPoint(collider.transform.position);
                if (viewPos.z < 0f) {
                    continue;
                }
                interactableList.Add(interactable);
            }
        }

        // 检测最近距离的npc
        IInteractable closestInteractable = null;
        foreach (IInteractable interactable in interactableList)
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
            } else {
                if (
                        Vector3.Distance(transform.position, interactable.GetTransform().position) <
                        Vector3.Distance(transform.position, closestInteractable.GetTransform().position)
                    )
                {
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }

    private IEnumerator useItem() {
        if (usingItem) {
            yield return null;
        } else {
            usingItem = !usingItem;
            item.Interact(gameObject);
            itemAnimation.SetTrigger("Use");

            yield return new WaitForSeconds(0.5f);

            showItemUI();
        }
    }

    private IEnumerator putAwayItem() {
        if (!usingItem) {
            yield return null;
        } else {
            usingItem = !usingItem;
            showItemUI();
            itemAnimation.SetTrigger("PutAway");

            yield return new WaitForSeconds(0.5f);

            item.Interact(gameObject);
        }
    }

    public void PutAwayItem() {
        StartCoroutine(putAwayItem());
    }

    public void showItemUI() {
        if (item != null) {
            if (item.isAllowedR() && RButton != null) {
                RButton.SetActive(usingItem);
            }
            if (item.isAllowedMouseLeft() && LeftButton != null) {
                LeftButton.SetActive(usingItem);
            }
        }
    }

    public bool IsUsingItem() {
        return usingItem;
    }

    public void setUI(bool value) {
        UIContainer.SetActive(value);
    }
}

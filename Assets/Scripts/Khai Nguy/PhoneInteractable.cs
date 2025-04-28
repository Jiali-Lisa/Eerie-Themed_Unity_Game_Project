using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneInteractable : ItemInteractable
{
    [SerializeField] private Camera phoneCamera;
    [SerializeField] private Camera outlineCamera;
    [SerializeField] private Material onMaterial;
    [SerializeField] private Material offMaterial;
    [SerializeField] private GameObject screen;
    [SerializeField] private string interactText;
    [SerializeField] private LayerMask offMode;
    [SerializeField] private LayerMask mode1;
    [SerializeField] private LayerMask mode2;
    [SerializeField] private LayerMask hiddenLayer;
    [SerializeField] private AudioSource cameraClick;
    [SerializeField] private GameObject cameraFlash;
    [SerializeField] private float flashTime;
    [SerializeField] private PhoneEffectsController effectController;
    [SerializeField] private ChasingTipsManager tipsManager;
    [SerializeField] private GameObject[] initTips;
    [SerializeField] private GameObject[] additionalTips;

    private GameObject player;
    private Renderer screenRenderer;
    private bool modeChanged = false;
    private bool canFlash = true;
    private bool flashed = false;
    private bool isPickedUp = false;

    void Start()
    {
        screenRenderer = screen.GetComponent<Renderer>();
        cameraFlash.SetActive(false);
        this.allowR = true;

        effectController.setGreyScale(false);

        tipsManager.setTips(initTips, true);
        tipsManager.setAllowTips(false);

        isPickedUp = false;
        canFlash = true;
        modeChanged = false;
        flashed = false;
        isUsing = false;
    }

    public override void Interact(GameObject player)
    {
        if (!isPickedUp) {
            tipsManager.setAllowTips(true);
            tipsManager.ShowTips();
            isPickedUp = true;
            return;
        }

        this.isUsing = !this.isUsing;
        if (this.isUsing) {
            tipsManager.HideTips();
            screenRenderer.material = onMaterial;
        } else {
            screenRenderer.material = offMaterial;
        }
        phoneCamera.gameObject.SetActive(this.isUsing);
        outlineCamera.gameObject.SetActive(this.isUsing);

        screen.transform.GetChild(0).gameObject.SetActive(this.isUsing);

        if (this.player == null) {
            this.player = transform.root.gameObject;
        }

        Rigidbody playerBody = this.player.GetComponent<Rigidbody>();
        if (offMode != 0 && mode1 != 0) {
            playerBody.excludeLayers = this.isUsing ? mode1 : offMode;
        }
        phoneCamera.cullingMask = ~mode1;
        outlineCamera.cullingMask = hiddenLayer;
        effectController.setGreyScale(false);
        modeChanged = false;
    }

    public override void InteractionOff() {}

    public override string GetInteractText()
    {
        return interactText == "" ? "Pick Up" : interactText;
    }

    public override Transform GetTransform()
    {
        return transform;
    }

    public override void UseR() {
        cameraClick.Play();
        StartCoroutine(shutter());
        Rigidbody playerBody = this.player.GetComponent<Rigidbody>();
        if (modeChanged) {
            // mode 1
            outlineCamera.cullingMask = hiddenLayer;
            phoneCamera.cullingMask = ~mode1;
            playerBody.excludeLayers = mode1;
            modeChanged = !modeChanged;
            effectController.setGreyScale(false);
        } else {
            // mode 2
            outlineCamera.cullingMask = (hiddenLayer | mode2);
            phoneCamera.cullingMask = ~mode2;
            playerBody.excludeLayers = mode2;
            modeChanged = !modeChanged;
            effectController.setGreyScale(true);
        }
    }

    private IEnumerator shutter() {
        screenRenderer.material = offMaterial;

        yield return new WaitForSeconds(flashTime);

        screenRenderer.material = onMaterial;
    }

    public override void UseMouseLeft() {
        if (this.isUsing && canFlash) {
            canFlash = false;
            StartCoroutine(mouseLeftAction());
        }
    }

    private IEnumerator flash() {
        cameraFlash.SetActive(true);
        screenRenderer.material = offMaterial;
        flashed = true;

        yield return new WaitForSeconds(flashTime);

        cameraFlash.SetActive(false);
        screenRenderer.material = onMaterial;
    }

    private IEnumerator mouseLeftAction() {
        cameraClick.Play();
        StartCoroutine(flash());
        RenderSettings.fogDensity = Mathf.Clamp(RenderSettings.fogDensity - 0.1f, 0f, 2f);

        yield return new WaitForSeconds(10);

        canFlash = true;
    }

    public void setFlashed(bool value) {
        flashed = value;
    }

    public bool hasFlashed() {
        return flashed;
    }

    public bool IsPickedUp() {
        return isPickedUp;
    }

    public void updateTips() {
        tipsManager.setTips(additionalTips, true);
    }
}

using System.Collections;
using UnityEngine;


public enum MovementAbility {
    Dash,
    Sprint,
}

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {

    private Vector3 playerMovementInput;
    private Vector2 playerMouseInput;
    private float xRotation;
    private float yRotation;
    [SerializeField] private Transform playerCameraHolder;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private MovementAbility ability = MovementAbility.Dash;
    [SerializeField] private float WalkSpeed = 5;
    [SerializeField] private float Sensitivty = 7;
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private bool enableHeadBob = false;
    [SerializeField] private float walkBobAmount = 0.1f;
    private float currentSpeed;
    private bool onGround = true;
    private bool playerJumped = false;
    private float timer;
    private Vector3 defaultCamPos;

    [SerializeField] private float sprintMult = 2.5f;

    [SerializeField] private float dashMult = 2.5f;
    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private float cooldown = 2f;
    private bool isDashing = false;
    private bool canDash = true;
    private float dashTimer;
    private bool playerDashed = false;

    private bool moveControl = true;
    private bool cameraMoveControl = true;

    [SerializeField] private AudioSource walkAudio;
    private bool walkAudioPlaying = false;

    private bool isWalking = false;
    private Animator camAnimator;

    private bool forceLookAt = false;
    private Transform forceLookAtTarget;


    private void Start() {
        currentSpeed = WalkSpeed;
        playerBody = GetComponent<Rigidbody>();
        yRotation = transform.rotation.eulerAngles.y;

        camAnimator = playerCamera.GetComponent<Animator>();
        defaultCamPos = playerCameraHolder.localPosition;
    }

    private void Update() {
        playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
        playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        playerJumped = Input.GetButton("Jump");
        if (ability == MovementAbility.Dash)
            playerDashed = Input.GetKeyDown(KeyCode.LeftShift);
        if (ability == MovementAbility.Sprint)
            HandleSprint();
        playerCamera.LookAt(focusTarget());
    }

    private void FixedUpdate() {
        if (moveControl){
            Move();
            if (enableHeadBob) HandleHeadBob();
        }
        if (cameraMoveControl){
            MoveCamera();
        }
    }

    private void Move()
    {
        if (playerMovementInput.sqrMagnitude > 0)
        {
            if (walkAudio != null) {
                if (!walkAudioPlaying) {
                    walkAudioPlaying = true;
                    StartCoroutine(playWalkAudio());
                }
            }
            Vector3 moveVector = transform.TransformDirection(playerMovementInput) * currentSpeed;
            if (isDashing)
            {
                moveVector *= dashMult;
            }

            playerBody.velocity = new Vector3(moveVector.x, playerBody.velocity.y, moveVector.z);
        }
        else
        {
            timer = 0;
            playerBody.velocity = new Vector3(0f, playerBody.velocity.y, 0f);
        }

        if (playerJumped && onGround)
        {
            playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onGround = false;
            playerJumped = false;
        }

        if (playerDashed && canDash)
        {
            StartCoroutine(Dash());
            playerDashed = false;
        }
    }

    private void MoveCamera() {
        if (playerMouseInput.x == 0 && playerMouseInput.y == 0) {
            return;
        }

        xRotation -= playerMouseInput.y * Sensitivty;
        yRotation += playerMouseInput.x * Sensitivty;

        xRotation = Mathf.Clamp(xRotation, -80, 60);

        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        playerCameraHolder.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "Floor") {
            onGround = true;
        }
    }

    public void enableControl(bool enable){
        moveControl = enable;
        cameraMoveControl = enable;
    }

    public void enableControl(bool enableMove, bool enableCameraMove){
        moveControl = enableMove;
        cameraMoveControl = enableCameraMove;
    }

    private IEnumerator Dash(){
        canDash = false;
        isDashing = true;
        dashTimer = dashTime;

        while (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
            yield return null;
        }

        isDashing = false;

        yield return new WaitForSeconds(cooldown);
        canDash = true;
    }

    private IEnumerator playWalkAudio() {
        if (currentSpeed == WalkSpeed) {
            walkAudio.volume = 0.5f;
        } else {
            walkAudio.volume = 1f;
        }
        walkAudio.Play();
        yield return new WaitForSeconds(Mathf.PI / currentSpeed);
        walkAudioPlaying = false;
    }

    private void HandleHeadBob() {
        if (playerMovementInput.sqrMagnitude > 0 && onGround) {
            timer += Time.deltaTime * currentSpeed;
            playerCameraHolder.localPosition = new Vector3(
                    defaultCamPos.x + Mathf.Cos(timer) * walkBobAmount * 2,
                    defaultCamPos.y + Mathf.Abs(Mathf.Sin(timer)) * walkBobAmount,
                    playerCameraHolder.localPosition.z
                );
            return;
        }
        if (playerCameraHolder.localPosition != defaultCamPos) {
            playerCameraHolder.localPosition = Vector3.Lerp(
                    playerCameraHolder.localPosition,
                    defaultCamPos,
                    Time.deltaTime * currentSpeed * 2
                );
        }
    }

    private Vector3 focusTarget() {
        Vector3 pos = new Vector3(
                transform.position.x,
                transform.position.y + playerCameraHolder.localPosition.y,
                transform.position.z
                );
        pos += playerCameraHolder.forward * 20.0f;
        return pos;
    }

    private void HandleSprint() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            currentSpeed = WalkSpeed * sprintMult;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            currentSpeed = WalkSpeed;
        }
    }

    public bool usingMovementAbility() {
        return currentSpeed != WalkSpeed || isDashing;
    }
}

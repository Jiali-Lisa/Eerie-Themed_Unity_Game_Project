using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    [SerializeField] private Transform playerObject;
    [SerializeField] private Transform playerCamera;

    [SerializeField] private float playerSpeed = 2.0f;
    //
    [SerializeField, Range(1f, 50f)] private float mouseSpeed = 15f;

    private float yRotation = 0f;

    private float gravity = -9.81f;
    private Vector3 gravityVector3;
    [SerializeField] private float velocity = 0.5f;
    private bool isGround;

    [SerializeField] private float jumpHeight = 2f;

    private CharacterController controller;

    //
    private bool control = true;



    [SerializeField] private float dashMult = 2.5f;
    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private float cooldown = 2f;
    private bool isDashing = false;
    private bool canDash = true;
    private float dashTimer;


    // Start is called before the first frame update
    void Start()
    {
        if (controller == null) controller = GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    { 
        Move();
        //
        if (control)
        {
            Turnaround();
            Jump();

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                StartCoroutine(Dash());
            }
        }
    }

    private void Move() {
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        float curSpeed = playerSpeed;
        if (isDashing){
            curSpeed *= dashMult;
        }


        Vector3 move = transform.right * moveX * curSpeed * Time.deltaTime + transform.forward * moveZ * curSpeed * Time.deltaTime;

        // 应用移动
        controller.Move(move);
    }

    private void Turnaround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSpeed;

        // turn horizontally, Vector3.up = (0, 1, 0)
        transform.Rotate(Vector3.up * mouseX);

        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -80, 80);

        playerCamera.localEulerAngles = new Vector3(yRotation, 0f, 0f);
    }

    private void Jump()
    {
        gravityVector3.y += velocity * gravity * Time.deltaTime;
  //      isGround = Physics.CheckSphere(gravityTransform.position, radius, gravityLayer);
        controller.Move(gravityVector3);

        


        if (Input.GetButtonDown("Jump") && controller.isGrounded == true)
        {
            gravityVector3.y = Mathf.Sqrt(jumpHeight * -1.0f * gravity);
            controller.Move(gravityVector3 * Time.deltaTime);


        }
        else if (controller.isGrounded == true)
        {
            gravityVector3.y = -0.1f;
            controller.Move(gravityVector3 * Time.deltaTime);
        }

    }


    public void enableControl(bool enable){
        control = enable;
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


}

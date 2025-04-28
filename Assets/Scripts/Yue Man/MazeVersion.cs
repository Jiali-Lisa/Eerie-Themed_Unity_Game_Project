using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeVersion : MonoBehaviour
{
    // every door's coordinate and turning angle 
    private Vector3[] doorPositions = new Vector3[]
    {
        new Vector3(-29.72f, 0.001540542f, -68.29f),
        new Vector3(-38.16f, 0.001540542f, -63.73f),
        new Vector3(-47.55f, 0.001540542f, -67.37f),
        new Vector3(-51.48f, 0.001540542f, -76.71f),
        new Vector3(-47.58f, 0.001540542f, -85.72f),
        new Vector3(-38.42f, 0.001540542f, -89.35f),
        new Vector3(-29.18f, 0.001540542f, -85.25f),
        new Vector3(-25.74f, 0.001540542f, -76.94f),
    };

    private int currentDoorIndex = 0;  
    private bool isTurning = false;  
    private Quaternion targetRotation;  
    private Transform cameraTransform;  
    private float rotationTime = 0.3f; 
    private float rotationProgress = 0f;  

    private void Start()
    {
        cameraTransform = Camera.main.transform;  
        RotateToDoor(currentDoorIndex);  
    }

    void Update()
    {
        // detect whether press left or right arrow 
        if (!isTurning)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                currentDoorIndex = (currentDoorIndex + 1) % doorPositions.Length;
                RotateToDoor(currentDoorIndex);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                currentDoorIndex = (currentDoorIndex - 1 + doorPositions.Length) % doorPositions.Length;
                RotateToDoor(currentDoorIndex);
            }
        }

        // turning to the target smoothly 
        if (isTurning)
        {
            rotationProgress += Time.deltaTime / rotationTime;  
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, targetRotation, rotationProgress);

            if (rotationProgress >= 1f)
            {
                cameraTransform.rotation = targetRotation;
                isTurning = false;  
            }
        }
    }

    // turning to the right door 
    void RotateToDoor(int doorIndex)
    {
        Vector3 directionToDoor = doorPositions[doorIndex] - cameraTransform.position;
        directionToDoor.y = 0;  

        targetRotation = Quaternion.LookRotation(directionToDoor);
        rotationProgress = 0f;
        isTurning = true;
    }
}

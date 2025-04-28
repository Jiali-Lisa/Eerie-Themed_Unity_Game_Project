using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField]private Camera playerCamera; 
    [SerializeField] private Cinemachine.CinemachineVirtualCamera virtualCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchToVirtualCamera();
        }
    }

    private void SwitchToVirtualCamera()
    {
        playerCamera.enabled = false; 
        virtualCamera.gameObject.SetActive(true); 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchBackToPlayerCamera();
        }
    }

    private void SwitchBackToPlayerCamera()
    {
        playerCamera.enabled = true; 
        virtualCamera.gameObject.SetActive(false); 
    }
}

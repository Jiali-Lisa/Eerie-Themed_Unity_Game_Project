using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingMovingWall : MonoBehaviour
{
    [SerializeField] private ChasingGameController gameController;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource lastWallAudio;
    private bool isOpen = false;
    private bool hinted = false;
    private float SphereCastRadius = 5f;

    void Update() {
        if (!hinted) {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, SphereCastRadius, transform.right * 10f, out hit))
            {
                if (hit.transform.gameObject.GetComponent<PlayerMovement>() != null)
                {
                    hinted = true;
                    gameController.GoalHint();
                }
            }
        }
    }

    public void Open()
    {
        animator.SetTrigger("Open");
        isOpen = true;

        if (lastWallAudio != null) {
            lastWallAudio.Play();
        }
        hinted = true;
    }

    public bool IsOpen() {
        return isOpen;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCursor : MonoBehaviour {

    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnApplicationFocus(bool hasFocus){
        if (hasFocus) {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
